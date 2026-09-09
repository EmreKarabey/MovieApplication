using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MoviesCategory.Commands.Update
{
    public class UpdatedMovieCategoryResponse
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }
    }
}
