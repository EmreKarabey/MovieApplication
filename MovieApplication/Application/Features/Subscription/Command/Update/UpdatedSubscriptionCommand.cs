using Application.Features.Subscription.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Subscription.Command.Update
{
    public class UpdatedSubscriptionCommand : IRequest<UpdatedSubscriptionResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid EntityID { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }

        public string? CacheKey => $"UpdatedSubscriptionCommand EntityID:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Subscription";
    }

    public class UpdateSubscriptionHandler : IRequestHandler<UpdatedSubscriptionCommand, UpdatedSubscriptionResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly SubscriptionBusinessRules _subscriptionBusinessRules;

        public UpdateSubscriptionHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, SubscriptionBusinessRules subscriptionBusinessRules)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _subscriptionBusinessRules = subscriptionBusinessRules;
        }

        public async Task<UpdatedSubscriptionResponse> Handle(UpdatedSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _subscriptionRepository.GetAsync(s => s.EntityID == request.EntityID);
            await _subscriptionBusinessRules.SubscriptionShouldExistWhenRequested(entity);

            _mapper.Map(request, entity);
            var updatedSubscription = await _subscriptionRepository.UpdateAsync(entity);
            var result = _mapper.Map<UpdatedSubscriptionResponse>(updatedSubscription);

            return result;
        }
    }
}
