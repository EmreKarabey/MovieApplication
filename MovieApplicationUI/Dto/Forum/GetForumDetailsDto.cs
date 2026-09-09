namespace MovieApplicationUI.Dto.Forum
{
    public class GetForumDetailsDto
    {
        public Guid EntityID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ForumCategoryId { get; set; }
        public string ForumCategorName { get; set; }
    }
}
