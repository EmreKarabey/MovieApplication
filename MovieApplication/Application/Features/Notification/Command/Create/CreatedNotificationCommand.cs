using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Notification.Command.Create
{
    public class CreatedNotificationCommand : IRequest<CreatedNotificationResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string? CacheKey => $"CreatedNotificationCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Notifications";
    }

    public class CreatedNotificationHandler : IRequestHandler<CreatedNotificationCommand, CreatedNotificationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly LoginBusinessRules _loginBusinessRules;
        private readonly IMapper _mapper;

        public CreatedNotificationHandler(IUserRepository userRepository, LoginBusinessRules loginBusinessRules, IMapper mapper, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _loginBusinessRules = loginBusinessRules;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<CreatedNotificationResponse> Handle(CreatedNotificationCommand request, CancellationToken cancellationToken)
        {
            await _loginBusinessRules.NoUserFound(request.UserId);

            var entity = _mapper.Map<Domain.Entities.Notification>(request);

            entity.CreatedAt = DateTime.UtcNow;
            entity.IsRead = false;

            var result = await _notificationRepository.AddAsync(entity);

            return _mapper.Map<CreatedNotificationResponse>(result);
        }
    }
}
