using System.Security.Claims;
using Application.Services;
using Application.Services.Repositories;
using CoreSecurity.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace MovieApplication.Hubs
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationHubService(INotificationRepository notificationRepository, IHubContext<SignalRHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendNotificationToUser(int userId)
        {
            var notificationList = await _notificationRepository.GetListAsync(predicate: n => n.UserID == userId);

            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotificationList", notificationList);
        }

        public async Task NotificationCount(int UserId)
        {
            var count = await _notificationRepository.MySendNotificationCount(UserId);
            await _hubContext.Clients.Group(UserId.ToString()).SendAsync("ReceiveNotificationCount", count);
        }
    }
}
