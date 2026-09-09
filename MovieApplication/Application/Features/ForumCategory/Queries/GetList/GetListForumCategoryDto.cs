using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ForumCategory.Queries.GetList
{
    public class GetListForumCategoryDto
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }
    }
}
