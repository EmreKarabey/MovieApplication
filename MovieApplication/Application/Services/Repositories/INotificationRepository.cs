using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using Domain.Entities;

namespace Application.Services.Repositories
{
    public interface INotificationRepository : IAsyncRepository<Notification, Guid>
    {
        public Task<int> MySendNotificationCount(int UserId);
        public Task<List<Notification>> MyNotificationList(int UserId);
    }
}
