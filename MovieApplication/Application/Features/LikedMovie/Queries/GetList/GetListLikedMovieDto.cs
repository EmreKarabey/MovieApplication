using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LikedMovie.Queries.GetList
{
    public class GetListLikedMovieDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
    }
}
