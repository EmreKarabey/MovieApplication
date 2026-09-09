using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Forum.Command.Update
{
    public class UpdateForumResponse
    {
        public Guid EntityID { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public Guid ForumCategoryId { get; set; }
    }
}
