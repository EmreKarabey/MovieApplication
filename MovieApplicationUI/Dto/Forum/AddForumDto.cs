namespace MovieApplicationUI.Dto.Forum
{
    public class AddForumDto
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public Guid ForumCategoryId { get; set; }
    }
}
