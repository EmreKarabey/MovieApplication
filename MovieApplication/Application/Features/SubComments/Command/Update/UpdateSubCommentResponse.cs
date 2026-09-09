using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SubComments.Command.Update
{
    public class UpdateSubCommentResponse
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public Guid CommentID { get; set; }
    }
}




