using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Forum.Queries.ActiveForumList
{
    public class GetActiveForumListDto
    {
        public Guid EntityID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Guid ForumCategoryId { get; set; }
        public string ForumCategoryName { get; set; }
    }
}
