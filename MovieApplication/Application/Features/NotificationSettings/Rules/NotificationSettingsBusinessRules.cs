using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.NotificationSettings.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.NotificationSettings.Rules
{
    public class NotificationSettingsBusinessRules : BaseBusinessRules
    {
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationSettingsBusinessRules(INotificationSettingsRepository notificationSettingsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task NoNotificationSettingsFound(Guid id)
        {
            Domain.Entities.NotificationSettings? notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.EntityID == id);

            if (notificationSettings == null) throw new BusinessException(NotificationSettingsMessages.NoNotificationSettingsFound);
        }

        public async Task UserAuthentication(Guid id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) throw new BusinessException(NotificationSettingsMessages.NotBeVerified);

            int currentUserId = int.Parse(userId);

            Domain.Entities.NotificationSettings? notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.EntityID == id);

            if (notificationSettings == null) throw new BusinessException(NotificationSettingsMessages.NoNotificationSettingsFound);

            if (notificationSettings.UserId != currentUserId) throw new BusinessException(NotificationSettingsMessages.NotBeVerified);
        }
    }
}
