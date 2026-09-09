namespace MovieApplicationUI.Dto.Subscription
{
    public class PublisherDataDto
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public string PublisherFirstName { get; set; }
        public string PublisherLastName { get; set; }
    }
}
