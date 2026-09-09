using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Application.Features.Subscription.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreSecurity.Entities;
using MediatR;

namespace Application.Features.Subscription.Command.Delete
{
    public class DeletedSubscriptionCommand : IRequest<DeletedSubscriptionResponse>, ICacheRemoveRequest, ILoggableRequest
    {
        public Guid EntityID { get; set; }

        public string? CacheKey => $"DeletedSubscriptionCommand EntityID:{EntityID}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Subscription";
    }

    public class DeleteSubscriptionHandler : IRequestHandler<DeletedSubscriptionCommand, DeletedSubscriptionResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;
        private readonly SubscriptionBusinessRules _subscriptionBusinessRules;

        public DeleteSubscriptionHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper, SubscriptionBusinessRules subscriptionBusinessRules)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _subscriptionBusinessRules = subscriptionBusinessRules;
        }

        public async Task<DeletedSubscriptionResponse> Handle(DeletedSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _subscriptionRepository.GetAsync(s => s.EntityID == request.EntityID);
            await _subscriptionBusinessRules.SubscriptionShouldExistWhenRequested(entity);

            var deletedSubscription = await _subscriptionRepository.DeleteAsync(entity);
            var result = _mapper.Map<DeletedSubscriptionResponse>(deletedSubscription);

            return result;
        }
    }
}
