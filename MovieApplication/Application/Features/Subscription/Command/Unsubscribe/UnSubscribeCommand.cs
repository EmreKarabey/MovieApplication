using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Features.Notification.Rules;
using Application.Features.NotificationSettings.Rules;
using Application.Features.Subscription.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Subscription.Command.Unsubscribe
{
    public class UnsubscribeCommand : IRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public int ChannelId { get; set; }
        public string? CacheKey => $"UnsubscribeCommand ChannelId:{ChannelId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubscriptionAndNotificationSettings";
    }
    public class UnsubscribeHandler : IRequestHandler<UnsubscribeCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly SubscriptionBusinessRules _subscriptionBusinessRules;
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly NotificationSettingsBusinessRules _notificationSettingsBusinessRules;

        public UnsubscribeHandler(IHttpContextAccessor httpContextAccessor, ISubscriptionRepository subscriptionRepository, LoginBusinessRules loginBusinessRules, SubscriptionBusinessRules subscriptionBusinessRules, INotificationSettingsRepository notificationSettingsRepository, NotificationSettingsBusinessRules notificationSettingsBusinessRules)
        {
            _httpContextAccessor = httpContextAccessor;
            _subscriptionRepository = subscriptionRepository;
            _loginBusinessRules = loginBusinessRules;
            _subscriptionBusinessRules = subscriptionBusinessRules;
            _notificationSettingsRepository = notificationSettingsRepository;
            _notificationSettingsBusinessRules = notificationSettingsBusinessRules;
        }

        public async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            var UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(UserId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = await _subscriptionRepository.GetAsync(predicate: n => n.ChannelId == request.ChannelId && n.UserId == currentUserId);

            await _subscriptionBusinessRules.NoSubscriptionFound(request.ChannelId, currentUserId);

            await _subscriptionRepository.DeleteAsync(entity, permanent: true);

            var notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.ChannelId == request.ChannelId && n.UserId == currentUserId);
            await _notificationSettingsBusinessRules.NoNotificationSettingsFound(notificationSettings.EntityID);

            await _notificationSettingsRepository.DeleteAsync(notificationSettings);
        }
    }
}
