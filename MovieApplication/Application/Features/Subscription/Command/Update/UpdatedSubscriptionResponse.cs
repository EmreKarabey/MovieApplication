using System;

namespace Application.Features.Subscription.Command.Update
{
    public class UpdatedSubscriptionResponse
    {
        public Guid EntityID { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
