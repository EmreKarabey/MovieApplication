using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Paginate;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Services.Repositories
{
    public interface IAnnouncementRepository : IAsyncRepository<Announcement, Guid>
    {
        public Task<List<Announcement>> Take2Async(Expression<Func<Announcement, bool>> predicate,
       Func<IQueryable<Announcement>,
       IIncludableQueryable<Announcement, object>>? include = null,
       Func<IQueryable<Announcement>, IOrderedQueryable<Announcement>>? orderBy = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default);

    }
}
