using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.NotificationSettings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;

namespace Application.Features.NotificationSettings.Command.Update
{
    public class UpdatedNotificationSettingsCommand : IRequest<UpdatedNotificationSettingsResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid EntityID { get; set; }
        public bool IsNotificationEnabled { get; set; }
        public int ChannelId { get; set; }

        public string? CacheKey => $"UpdatedNotificationSettingsCommand Id:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "NotificationSettings";
    }

    public class UpdatedNotificationSettingsHandler : IRequestHandler<UpdatedNotificationSettingsCommand, UpdatedNotificationSettingsResponse>
    {
        private readonly NotificationSettingsBusinessRules _notificationSettingsBusinessRules;
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly IMapper _mapper;

        public UpdatedNotificationSettingsHandler(NotificationSettingsBusinessRules notificationSettingsBusinessRules, INotificationSettingsRepository notificationSettingsRepository, IMapper mapper)
        {
            _notificationSettingsBusinessRules = notificationSettingsBusinessRules;
            _notificationSettingsRepository = notificationSettingsRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedNotificationSettingsResponse> Handle(UpdatedNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            await _notificationSettingsBusinessRules.NoNotificationSettingsFound(request.EntityID);

            Domain.Entities.NotificationSettings? notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.EntityID == request.EntityID);

            notificationSettings = _mapper.Map(request, notificationSettings);

            var updateEntity = await _notificationSettingsRepository.UpdateAsync(notificationSettings);

            var result = _mapper.Map<UpdatedNotificationSettingsResponse>(updateEntity);

            return result;
        }
    }
}
