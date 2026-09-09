using Application.Features.NotificationSettings.Command.Create;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Subscription.Command.Create
{
    public class CreatedSubscriptionCommand : IRequest<(CreatedSubscriptionResponse, CreatedNotificationSettingsResponse)>, ITransactionalRequest, ILoggableRequest, ICacheRemoveRequest
    {
        public int ChannelId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"CreatedSubscriptionCommand ChannelId:{ChannelId}, UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "SubscriptionAndNotificationSettings";
    }

    public class CreateSubscriptionHandler : IRequestHandler<CreatedSubscriptionCommand, (CreatedSubscriptionResponse, CreatedNotificationSettingsResponse)>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly IMapper _mapper;

        public CreateSubscriptionHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, INotificationSettingsRepository notificationSettingsRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _notificationSettingsRepository = notificationSettingsRepository;
        }

        public async Task<(CreatedSubscriptionResponse, CreatedNotificationSettingsResponse)> Handle(CreatedSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Subscription>(request);
            var addedSubscription = await _subscriptionRepository.AddAsync(entity);
            var result = _mapper.Map<CreatedSubscriptionResponse>(addedSubscription);

            var notificationSettings = new Domain.Entities.NotificationSettings
            {
                ChannelId = request.ChannelId,
                CreatedAt = DateTime.UtcNow,
                UserId = request.UserId,
                IsNotificationEnabled = false
            };

            var AddNotificationSettings = await _notificationSettingsRepository.AddAsync(notificationSettings);
            var result2 = _mapper.Map<CreatedNotificationSettingsResponse>(AddNotificationSettings);

            return (result, result2);
        }
    }
}
