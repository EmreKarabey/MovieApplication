namespace MovieApplicationUI.Dto.Subscription
{
    public class AddSubscriptionDto
    {
        public int ChannelId { get; set; }
        public int UserId { get; set; }

        public Guid MovieID { get; set; }
        public string videoUrl { get; set; }
        public string movieName { get; set; }
    }
}
