using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.NotificationSettings.Command.Create
{
    public class CreatedNotificationSettingsCommand : IRequest<CreatedNotificationSettingsResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public int ChannelId { get; set; }
        public bool IsNotificationEnabled { get; set; } = true;

        public string? CacheKey => $"CreatedNotificationSettingsCommand User_Id";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "NotificationSettings";
    }

    public class CreatedNotificationSettingsHandler : IRequestHandler<CreatedNotificationSettingsCommand, CreatedNotificationSettingsResponse>
    {
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginBusinessRules _loginBusinessRules;

        public CreatedNotificationSettingsHandler(INotificationSettingsRepository notificationSettingsRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, LoginBusinessRules loginBusinessRules)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<CreatedNotificationSettingsResponse> Handle(CreatedNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = _mapper.Map<Domain.Entities.NotificationSettings>(request);
            entity.UserId = currentUserId;

            var addEntity = await _notificationSettingsRepository.AddAsync(entity);

            var result = _mapper.Map<CreatedNotificationSettingsResponse>(addEntity);

            return result;
        }
    }
}
