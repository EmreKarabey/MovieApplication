using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Rules;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreSecurity.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Subscription.Queries.IsSubscribe
{
    public class GetIsSubscribeQuery : IRequest<bool>, ILoggableRequest
    {
        public int ChannelId { get; set; }
    }
    public class GetIsSubscribeHandler : IRequestHandler<GetIsSubscribeQuery, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly LoginBusinessRules _loginBusinessRules;

        public GetIsSubscribeHandler(IHttpContextAccessor httpContextAccessor, ISubscriptionRepository subscriptionRepository, LoginBusinessRules loginBusinessRules)
        {
            _httpContextAccessor = httpContextAccessor;
            _subscriptionRepository = subscriptionRepository;
            _loginBusinessRules = loginBusinessRules;
        }

        public async Task<bool> Handle(GetIsSubscribeQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(userId);

            await _loginBusinessRules.NoUserFound(currentUserId);

            var entity = await _subscriptionRepository.GetAsync(predicate: n => n.ChannelId == request.ChannelId && n.UserId == currentUserId);

            if (entity != null) return true;

            return false;
        }
    }
}
