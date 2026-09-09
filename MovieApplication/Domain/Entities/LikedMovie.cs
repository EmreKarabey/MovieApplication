using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class LikedMovie : Entity<Guid>
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }


        public LikedMovie() { }


        public LikedMovie(Guid Id, Guid movieID, int userID) : this()
        {
            EntityID = Id;
            MovieID = movieID;
            UserID = userID;
        }
    }
}
