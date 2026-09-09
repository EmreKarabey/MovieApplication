using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Command.Create
{
    public class CreatedCommentResponse
    {
        public Guid EntityID { get; set; }
        public string Content { get; set; }
        public int UserID { get; set; }
        public Guid MovieID { get; set; }
    }
}
