using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Category.Command.Update
{
    public class UpdatedCategoryResponse
    {
        public Guid EntityID { get; set; }
        public string CategoryName { get; set; }
    }
}
