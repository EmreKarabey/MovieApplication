using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FavoriteMovie.Queries.GetList
{
    public class GetListFavoriteMovieDto
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
    }
}
