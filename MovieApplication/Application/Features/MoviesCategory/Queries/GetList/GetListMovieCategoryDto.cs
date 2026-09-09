using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MoviesCategory.Queries.GetList
{
    public class GetListMovieCategoryDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }
    }
}
