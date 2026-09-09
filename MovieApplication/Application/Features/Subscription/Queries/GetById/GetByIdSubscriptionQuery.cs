using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Subscription.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Subscription.Queries.GetById
{
    public class GetByIdSubscriptionQuery : IRequest<GetByIdSubscriptionDto>, ICachableRequest, ILoggableRequest
    {
        public Guid EntityID { get; set; }

        public string? CacheKey => $"GetByIdSubscriptionQuery EntityId:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Subcription";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetByIdSubscriptionQueryHandler : IRequestHandler<GetByIdSubscriptionQuery, GetByIdSubscriptionDto>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly SubscriptionBusinessRules _subscriptionBusinessRules;

        public GetByIdSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, SubscriptionBusinessRules subscriptionBusinessRules)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _subscriptionBusinessRules = subscriptionBusinessRules;
        }

        public async Task<GetByIdSubscriptionDto> Handle(GetByIdSubscriptionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _subscriptionRepository.GetAsync(s => s.EntityID == request.EntityID);
            await _subscriptionBusinessRules.SubscriptionShouldExistWhenRequested(entity);

            var result = _mapper.Map<GetByIdSubscriptionDto>(entity);
            return result;
        }
    }
}
