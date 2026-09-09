using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Notification.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.Notification.Command.Delete
{
    public class DeletedNotificationCommand : IRequest<DeletedNotificationResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid Id { get; set; }

        public string? CacheKey => $"DeletedNotificationCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Notifications";
    }

    public class DeletedNotificationHandler : IRequestHandler<DeletedNotificationCommand, DeletedNotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly NotificationBusinessRules _notificationBusinessRules;

        public DeletedNotificationHandler(INotificationRepository notificationRepository, IMapper mapper, NotificationBusinessRules notificationBusinessRules)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _notificationBusinessRules = notificationBusinessRules;
        }

        public async Task<DeletedNotificationResponse> Handle(DeletedNotificationCommand request, CancellationToken cancellationToken)
        {
            await _notificationBusinessRules.NoNotificationFound(request.Id);

            Domain.Entities.Notification notification = await _notificationRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            var deleteNotification = await _notificationRepository.DeleteAsync(notification, permanent: true);

            var result = _mapper.Map<DeletedNotificationResponse>(deleteNotification);

            return result;
        }
    }

}
