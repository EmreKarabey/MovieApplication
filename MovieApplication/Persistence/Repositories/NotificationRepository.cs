using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CorePersistence.Paginate;
using CorePersistence.Repositories;
using CoreSecurity.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    public class NotificationRepository : EFRepositoryBase<Notification, Guid, BaseDBContext>, INotificationRepository
    {
        public NotificationRepository(BaseDBContext context) : base(context)
        {
        }

        public async Task<List<Notification>> MyNotificationList(int UserId)
        {
            var list = _context.Notifications.AsQueryable();

            var result = await list.Where(n => n.UserID == UserId).ToListAsync();

            return result;
        }

        public async Task<int> MySendNotificationCount(int UserId)
        {
            return await _context.Notifications.Where(n => n.UserID == UserId && !n.IsRead).CountAsync();
        }
    }
}
