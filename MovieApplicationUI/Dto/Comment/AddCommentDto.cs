namespace MovieApplicationUI.Dto.Comment
{
    public class AddCommentDto
    {
        public string Content { get; set; }
        public Guid MovieID { get; set; }
        public int UserId { get; set; }
        public string videoUrl { get; set; }
        public string movieName { get; set; }
    }
}
