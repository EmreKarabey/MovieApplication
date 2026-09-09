namespace MovieApplicationUI.Dto.Activities
{
    public class MyActivityDto
    {
        public Guid Id { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }

        public string MovieName { get; set; }
        public string ImageURL { get; set; }
        public int ActivitiesCategory { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
