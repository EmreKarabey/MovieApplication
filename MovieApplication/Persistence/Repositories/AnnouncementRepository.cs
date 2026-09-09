using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Paginate;
using CorePersistence.Paging;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class AnnouncementRepository : EFRepositoryBase<Announcement, Guid, BaseDBContext>, IAnnouncementRepository
    {
        public AnnouncementRepository(BaseDBContext context) : base(context)
        {
        }

        public async Task<List<Announcement>> Take2Async(Expression<Func<Announcement, bool>> predicate, Func<IQueryable<Announcement>, IIncludableQueryable<Announcement, object>>? include = null, Func<IQueryable<Announcement>, IOrderedQueryable<Announcement>>? orderBy = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<Announcement> list = Query();

            if (!enableTracking) list = list.AsNoTracking();
            if (withDeleted) list = list.IgnoreQueryFilters();
            if (include != null) list = include(list);
            if (predicate != null) list = list.Where(predicate);

            list = list.OrderByDescending(n => n.CreatedAt);

            return await list.ToListAsync();
        }
    }
}
