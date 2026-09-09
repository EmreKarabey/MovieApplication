using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Queries.GetById
{
    public class GetByIdCategoryDto
    {
        public Guid EntityId { get; set; }
        public string CategoryName { get; set; }
    }
}
