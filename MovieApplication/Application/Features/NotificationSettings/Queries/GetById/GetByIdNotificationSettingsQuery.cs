using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.NotificationSettings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.NotificationSettings.Queries.GetById
{
    public class GetByIdNotificationSettingsQuery : IRequest<GetByIdNotificationSettingsDto>, ILoggableRequest
    {
        public Guid EntityID { get; set; }
    }

    public class GetByIdNotificationSettingsQueryHandler : IRequestHandler<GetByIdNotificationSettingsQuery, GetByIdNotificationSettingsDto>
    {
        private readonly IMapper _mapper;
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly NotificationSettingsBusinessRules _notificationSettingsBusinessRules;

        public GetByIdNotificationSettingsQueryHandler(IMapper mapper, INotificationSettingsRepository notificationSettingsRepository, NotificationSettingsBusinessRules notificationSettingsBusinessRules)
        {
            _mapper = mapper;
            _notificationSettingsRepository = notificationSettingsRepository;
            _notificationSettingsBusinessRules = notificationSettingsBusinessRules;
        }

        public async Task<GetByIdNotificationSettingsDto> Handle(GetByIdNotificationSettingsQuery request, CancellationToken cancellationToken)
        {
            await _notificationSettingsBusinessRules.NoNotificationSettingsFound(request.EntityID);

            Domain.Entities.NotificationSettings? notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.EntityID == request.EntityID, cancellationToken: cancellationToken);

            GetByIdNotificationSettingsDto response = _mapper.Map<GetByIdNotificationSettingsDto>(notificationSettings);

            return response;
        }
    }
}
