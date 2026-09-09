using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MoviesCategory.Queries.GetMovieId
{
    public class GetByMovieDto
    {
        public ICollection<string> CategoryName { get; set; }
        public string MovieName { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
    }
}
