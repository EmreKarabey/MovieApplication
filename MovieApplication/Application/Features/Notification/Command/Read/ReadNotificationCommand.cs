using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Notification.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.Notification.Command.Read
{
    public class ReadNotificationCommand : IRequest, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid EntityId { get; set; }

        public string? CacheKey => $"ReadNotificationCommand EntityId:{EntityId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Notifications";
    }
    public class ReadNotificationHandler : IRequestHandler<ReadNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationBusinessRules _notificationBusinessRules;

        public ReadNotificationHandler(INotificationRepository notificationRepository, NotificationBusinessRules notificationBusinessRules)
        {
            _notificationRepository = notificationRepository;
            _notificationBusinessRules = notificationBusinessRules;
        }

        public async Task Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notificationBusinessRules.NoNotificationFound(request.EntityId);

            var entity = await _notificationRepository.GetAsync(predicate: n => n.EntityID == request.EntityId);

            entity.IsRead = true;

            await _notificationRepository.UpdateAsync(entity);
        }
    }
}
