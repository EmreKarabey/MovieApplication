using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UnlikedMovie.Queries.SearchUnlikedMovie
{
    public class GetSearchUnlikedMovieDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public IEnumerable<string> CategoryName { get; set; }
    }
}
