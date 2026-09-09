using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SavedMovie.Command.Create
{
    public class CreatedSavedMovieResponse
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
    }
}
