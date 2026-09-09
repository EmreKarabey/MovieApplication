namespace MovieApplicationUI.Dto.Comment
{
    public class CommentListDto
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SubCommentsCount { get; set; }
    }
}
