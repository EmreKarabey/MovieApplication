namespace MovieApplicationUI.Dto.SubComment
{
    public class AddSubCommentDto
    {
        public string Content { get; set; }

        public int UserID { get; set; }
        public Guid CommentID { get; set; }
    }
}
