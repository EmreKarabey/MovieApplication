using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class FavoriteMovie : Entity<Guid>
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }

        public FavoriteMovie() { }

        public FavoriteMovie(Guid Id, Guid movieId, int UserId) : this()
        {
            EntityID = Id;
            MovieID = movieId;
            UserID = UserId;
        }
    }
}
