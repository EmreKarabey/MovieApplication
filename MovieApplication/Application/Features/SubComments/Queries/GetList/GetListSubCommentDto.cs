using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SubComments.Queries.GetList
{
    public class GetListSubCommentDto
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Guid CommentID { get; set; }
    }
}




