using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Queries.GetList
{
    public class GetListCategoryDto
    {
        public Guid EntityId { get; set; }
        public string CategoryName { get; set; }
    }
}
