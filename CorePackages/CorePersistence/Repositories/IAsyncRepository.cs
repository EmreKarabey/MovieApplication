using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Dynamic;
using CorePersistence.Paginate;
using Microsoft.EntityFrameworkCore.Query;

namespace CorePersistence.Repositories
{
    public interface IAsyncRepository<TEntity, TEntityID> : IQuery<TEntity> where TEntity : Entity<TEntityID>
    {
        public Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        public Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);

        Task<Paginate<TEntity>> GetListByDynamicAsync(
         DynamicQuery dynamic,
         Expression<Func<TEntity, bool>>? predicate = null,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
         int index = 0,
         int size = 10,
         bool withDeleted = false,
         bool enableTracking = true,
         CancellationToken cancellationToken = default
     );

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null,
            bool withDeleted = false,
            bool enableTracking = true,
           CancellationToken cancellationToken = default);

        public Task<TEntity> AddAsync(TEntity entity);
        public Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
        public Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
        public Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);
        public Task<TEntity> UpdateAsync(TEntity entity);
        public Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
    }

 
}
