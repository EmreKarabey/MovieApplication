using Application.Services.Repositories;
using CorePersistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SubscriptionRepository : EFRepositoryBase<Subscription, Guid, BaseDBContext>, ISubscriptionRepository
    {
        public SubscriptionRepository(BaseDBContext context) : base(context)
        {

        }

        public async Task<List<Subscription>> ListAsync(int ChannelId)
        {
            var list = _context.Set<Subscription>().AsQueryable();

            var result = await list.Where(n => n.ChannelId == ChannelId).ToListAsync();

            return result;
        }
    }
}
