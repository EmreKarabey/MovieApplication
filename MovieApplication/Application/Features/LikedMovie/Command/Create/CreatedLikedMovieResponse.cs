using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LikedMovie.Command.Create
{
    public class CreatedLikedMovieResponse
    {
        public Guid EntityID { get; set; }
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
    }
}
