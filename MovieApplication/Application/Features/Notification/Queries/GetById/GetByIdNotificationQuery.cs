using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Notification.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.Notification.Queries.GetById
{
    public class GetByIdNotificationQuery : IRequest<GetByIdNotificationDto>, ILoggableRequest
    {
        public Guid EntityID { get; set; }
    }

    public class GetByIdNotificationQueryHandler : IRequestHandler<GetByIdNotificationQuery, GetByIdNotificationDto>
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationBusinessRules _notificationBusinessRules;

        public GetByIdNotificationQueryHandler(IMapper mapper, INotificationRepository notificationRepository, NotificationBusinessRules notificationBusinessRules)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _notificationBusinessRules = notificationBusinessRules;
        }

        public async Task<GetByIdNotificationDto> Handle(GetByIdNotificationQuery request, CancellationToken cancellationToken)
        {
            await _notificationBusinessRules.NoNotificationFound(request.EntityID);

            Domain.Entities.Notification? notification = await _notificationRepository.GetAsync(predicate: n => n.EntityID == request.EntityID, cancellationToken: cancellationToken);

            GetByIdNotificationDto response = _mapper.Map<GetByIdNotificationDto>(notification);

            return response;
        }
    }
}
