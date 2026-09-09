using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Movies.Queries.GetById
{
    public class GetByIdMovieDto
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public string PublisherFirstName { get; set; }
        public string PublisherLastName { get; set; }
    }
}
