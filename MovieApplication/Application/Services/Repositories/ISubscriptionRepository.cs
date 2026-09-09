using CorePersistence.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories
{
    public interface ISubscriptionRepository : IAsyncRepository<Subscription, Guid>
    {
        public Task<List<Subscription>> ListAsync(int ChannelId);
    }
}
