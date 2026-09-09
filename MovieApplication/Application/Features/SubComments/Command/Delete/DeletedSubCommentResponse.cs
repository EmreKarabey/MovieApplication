using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SubComments.Command.Delete
{
    public class DeletedSubCommentResponse
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public Guid CommentID { get; set; }
    }
}




