using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.SavedMovie.Constants;
using Application.Features.Subscription.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;

namespace Application.Features.Subscription.Rules
{
    public class SubscriptionBusinessRules : BaseBusinessRules
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionBusinessRules(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task SubscriptionShouldExistWhenRequested(Domain.Entities.Subscription? subscription)
        {
            if (subscription == null) throw new BusinessException("Requested subscription does not exist.");
        }

        public async Task NoSubscriptionFound(int ChannelId, int UserId)
        {
            Domain.Entities.Subscription subscription = await _subscriptionRepository.GetAsync(predicate: n => n.ChannelId == ChannelId && n.UserId == UserId);

            if (subscription == null) throw new BusinessException(SubscriptionMessages.NoSubscriptionFound);
        }
    }
}
