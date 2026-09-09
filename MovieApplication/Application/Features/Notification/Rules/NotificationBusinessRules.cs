using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Notification.Constants;
using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Notification.Rules
{
    public class NotificationBusinessRules
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationBusinessRules(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoNotificationFound(Guid Id)
        {
            Domain.Entities.Notification notification = await _notificationRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (notification == null) throw new BusinessException(NotificationMessages.NoNotificationFound);
        }

        public async Task UserAuthentication(Guid Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);

            Domain.Entities.Notification notification = await _notificationRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (notification.UserID != currentUserId) throw new BusinessException(NotificationMessages.NotBeVerified);
        }
    }
}
