using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Dynamic;
using CorePersistence.Paginate;
using CorePersistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace CorePersistence.Repositories
{
    public class EFRepositoryBase<TEntity, TEntityID, TContext> : IAsyncRepository<TEntity, TEntityID> where TEntity : Entity<TEntityID> where TContext : DbContext
    {
        protected readonly TContext _context;

        public EFRepositoryBase(TContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
        {
            foreach (var item in entities)
                item.CreatedAt = DateTime.UtcNow;

            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> entities = Query();

            if (!enableTracking) entities = entities.AsNoTracking();
            if (withDeleted) entities = entities.IgnoreQueryFilters();
            if (predicate != null) entities = entities.Where(predicate);
            return await entities.AnyAsync(cancellationToken);
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
        {
            await SetEntityAsDeletedAsync(entity, permanent);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false)
        {
            await setEntityAsDeletedAsync(entities, permanent);
            await _context.SaveChangesAsync();

            return entities;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var queries = Query();
            if (!enableTracking) queries = queries.AsNoTracking();
            if (withDeleted) queries = queries.IgnoreQueryFilters();
            if (include != null) queries = include(queries);

            return await queries.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<Paginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var list = Query().ToDynamic(dynamic);

            if (!enableTracking) list = list.AsNoTracking();
            if (withDeleted) list = list.IgnoreQueryFilters();
            if (include != null) list = include(list);
            if (predicate != null) list = list.Where(predicate);

            return await list.ToPaginateAsync(index: index, size: size, cancellationToken: cancellationToken);
        }


        public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> list = Query();

            if (!enableTracking) list = list.AsNoTracking();
            if (withDeleted) list = list.IgnoreQueryFilters();
            if (include != null) list = include(list);
            if (predicate != null) list = list.Where(predicate);

            if (orderBy != null) return await orderBy(list).ToPaginateAsync(index, size, cancellationToken);

            return await list.ToPaginateAsync(index, size, cancellationToken);
        }

        public IQueryable<TEntity> Query() => _context.Set<TEntity>();

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                entity.UpdatedAt = DateTime.UtcNow;
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        #region(Yardımcı Metodlar)

        protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent = false)
        {
            if (!permanent)
            {
                ChechkHasEntityHaveOneToOneRelation(entity);
                await SetEntityAsSoftDeletedAsync(entity);
            }
            else
            {
                _context.Remove(entity);
            }
        }

        private async Task SetEntityAsSoftDeletedAsync(IEntityTimeStamps entity)
        {
            if (entity.DeletedAt.HasValue)
                return;
            entity.DeletedAt = DateTime.UtcNow;

            var navigations = _context
                .Entry(entity)
                .Metadata.GetNavigations()
                .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
                .ToList();
            foreach (INavigation? navigation in navigations)
            {
                if (navigation.TargetEntityType.IsOwned())
                    continue;
                if (navigation.PropertyInfo == null)
                    continue;

                object? navValue = navigation.PropertyInfo.GetValue(entity);
                if (navigation.IsCollection)
                {
                    if (navValue == null)
                    {
                        IQueryable query = _context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                        if (navValue == null)
                            continue;
                    }

                    foreach (IEntityTimeStamps navValueItem in (IEnumerable)navValue)
                        await SetEntityAsSoftDeletedAsync(navValueItem);
                }
                else
                {
                    if (navValue == null)
                    {
                        IQueryable query = _context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                        navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                            .FirstOrDefaultAsync();
                        if (navValue == null)
                            continue;
                    }

                    await SetEntityAsSoftDeletedAsync((IEntityTimeStamps)navValue);
                }
            }

            _context.Update(entity);
        }

        protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
        {
            Type queryProviderType = query.Provider.GetType();
            MethodInfo createQueryMethod =
                queryProviderType
                    .GetMethods()
                    .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                    ?.MakeGenericMethod(navigationPropertyType)
                ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
            var queryProviderQuery =
                (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
            return queryProviderQuery.Where(x => !((IEntityTimeStamps)x).DeletedAt.HasValue);
        }


        protected void ChechkHasEntityHaveOneToOneRelation(TEntity entity)
        {
            bool HasEntityOneToOneRelation = _context.Entry(entity)
                .Metadata
                .GetForeignKeys()
                .All(x => x.DependentToPrincipal?.IsCollection == true || x.PrincipalToDependent?.IsCollection == true || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()) == false;
            if (HasEntityOneToOneRelation)
                throw new InvalidOperationException(
                    "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key."
                );
        }


        protected async Task setEntityAsDeletedAsync(ICollection<TEntity> entities, bool permanent)
        {
            foreach (var item in entities) await SetEntityAsDeletedAsync(item, permanent);
        }




        #endregion
    }
}
