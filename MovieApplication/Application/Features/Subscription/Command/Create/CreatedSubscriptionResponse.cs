using System;

namespace Application.Features.Subscription.Command.Create
{
    public class CreatedSubscriptionResponse
    {
        public Guid EntityID { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
