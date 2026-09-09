using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ForumCategory.Command.Update
{
    public class UpdatedForumCategoryResponse
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
    }
}
