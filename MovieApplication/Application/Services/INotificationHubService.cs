using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INotificationHubService
    {
        Task SendNotificationToUser(int userId);
        Task NotificationCount(int UserId);
    }
}
