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

namespace Application.Features.Notification.Command.Update
{
    public class UpdateNotificationCommand : IRequest<UpdateNotificationResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid Id { get; set; }

        public bool IsRead { get; set; }

        public string? CacheKey => $"UpdateNotificationCommand Id:{Id}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Notifications";
    }

    public class UpdateNotificationHandler : IRequestHandler<UpdateNotificationCommand, UpdateNotificationResponse>
    {
        private readonly NotificationBusinessRules _notificationBusinessRules;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public UpdateNotificationHandler(NotificationBusinessRules notificationBusinessRules, INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationBusinessRules = notificationBusinessRules;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<UpdateNotificationResponse> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {

            await _notificationBusinessRules.NoNotificationFound(request.Id);

            Domain.Entities.Notification notification = await _notificationRepository.GetAsync(predicate: n => n.EntityID == request.Id);

            notification = _mapper.Map(request, notification);

            var updateNotification = await _notificationRepository.UpdateAsync(notification);

            var result = _mapper.Map<UpdateNotificationResponse>(updateNotification);

            return result;
        }
    }

}
