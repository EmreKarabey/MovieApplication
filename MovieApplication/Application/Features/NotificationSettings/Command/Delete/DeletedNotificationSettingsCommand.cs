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

namespace Application.Features.NotificationSettings.Command.Delete
{
    public class DeletedNotificationSettingsCommand : IRequest<DeletedNotificationSettingsResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public Guid EntityID { get; set; }

        public string? CacheKey => $"DeletedNotificationSettingsCommand Id:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "NotificationSettings";
    }

    public class DeletedNotificationSettingsHandler : IRequestHandler<DeletedNotificationSettingsCommand, DeletedNotificationSettingsResponse>
    {
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly NotificationSettingsBusinessRules _notificationSettingsBusinessRules;
        private readonly IMapper _mapper;

        public DeletedNotificationSettingsHandler(INotificationSettingsRepository notificationSettingsRepository, NotificationSettingsBusinessRules notificationSettingsBusinessRules, IMapper mapper)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _notificationSettingsBusinessRules = notificationSettingsBusinessRules;
            _mapper = mapper;
        }

        public async Task<DeletedNotificationSettingsResponse> Handle(DeletedNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            await _notificationSettingsBusinessRules.NoNotificationSettingsFound(request.EntityID);

            Domain.Entities.NotificationSettings? notificationSettings = await _notificationSettingsRepository.GetAsync(predicate: n => n.EntityID == request.EntityID);

            var deleteEntity = await _notificationSettingsRepository.DeleteAsync(notificationSettings);

            var result = _mapper.Map<DeletedNotificationSettingsResponse>(deleteEntity);

            return result;
        }
    }
}
