using System;

namespace Application.Features.Subscription.Queries.GetList
{
    public class GetListSubscriptionDto
    {
        public Guid EntityID { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
