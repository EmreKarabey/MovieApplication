using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.History.Queries.MyStatics
{
    public class GetMyStaticsMovieDto
    {
        public int WatchMovieCount { get; set; }
        public int FavoriteMovieCount { get; set; }
        public int SaveMovieCount { get; set; }
        public int MovieHoursCount { get; set; }
    }
}
