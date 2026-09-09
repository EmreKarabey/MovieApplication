using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SubComments.Queries.GetCommentId
{
    public class GetByCommentSubCommentIdDto
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Guid CommentID { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}




