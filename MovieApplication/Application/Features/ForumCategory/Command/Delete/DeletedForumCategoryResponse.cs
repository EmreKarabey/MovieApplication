using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ForumCategory.Command.Delete
{
    public class DeletedForumCategoryResponse
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
    }
}
