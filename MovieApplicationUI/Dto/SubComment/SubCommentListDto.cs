using System;

namespace MovieApplicationUI.Dto.SubComment
{
    public class SubCommentListDto
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Guid CommentID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
