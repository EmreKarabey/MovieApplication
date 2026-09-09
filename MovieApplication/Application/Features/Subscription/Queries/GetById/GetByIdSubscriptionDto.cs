using System;

namespace Application.Features.Subscription.Queries.GetById
{
    public class GetByIdSubscriptionDto
    {
        public Guid EntityID { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
