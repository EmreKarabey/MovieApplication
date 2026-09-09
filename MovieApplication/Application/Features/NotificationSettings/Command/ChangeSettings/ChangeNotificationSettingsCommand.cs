using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.NotificationSettings.Command.Update;
using Application.Features.NotificationSettings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;

namespace Application.Features.NotificationSettings.Command.ChangeSettings
{
    public class ChangeNotificationSettingsCommand : IRequest<ChangeNotificationSettingsResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public int UserId { get; set; }
        public int ChannelId { get; set; }

        public string? CacheKey => $"ChangeNotificationSettingsCommand UserId:{UserId} ChannelId:{ChannelId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "NotificationSettings";
    }
    public class UpdatedNotificationSettingsHandler : IRequestHandler<ChangeNotificationSettingsCommand, ChangeNotificationSettingsResponse>
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

        public async Task<ChangeNotificationSettingsResponse> Handle(ChangeNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _notificationSettingsRepository.GetAsync(predicate: n => n.ChannelId == request.ChannelId && n.UserId == request.UserId);
            await _notificationSettingsBusinessRules.NoNotificationSettingsFound(entity.EntityID);

            entity.IsNotificationEnabled = !entity.IsNotificationEnabled;

            var updateEntity = await _notificationSettingsRepository.UpdateAsync(entity);

            var result = _mapper.Map<ChangeNotificationSettingsResponse>(updateEntity);

            return result;
        }
    }
}
