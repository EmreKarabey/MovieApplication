using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class HistoryRepository : EFRepositoryBase<History, Guid, BaseDBContext>, IHistoryRepository
    {
        public HistoryRepository(BaseDBContext context) : base(context)
        {
        }

        public async Task<int> WatchedHoursMovie(int UserId)
        {
            IQueryable<History> list = Query();

            var totalSeconds = await list.Where(n => n.UserID == UserId).SumAsync(n => n.WatchedSeconds);

            var totalHours = totalSeconds / 3600;

            return totalHours;
        }

        public async Task<int> WatchedMovieCount(int UserId)
        {
            IQueryable<History> list = Query();

            var result = await list.Where(n => n.UserID == UserId).CountAsync();

            return result;
        }
    }
}
