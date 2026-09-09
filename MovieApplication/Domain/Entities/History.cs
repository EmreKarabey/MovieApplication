using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class History : Entity<Guid>
    {
        public Guid MovieID { get; set; }
        public int UserID { get; set; }

        public int TotalSeconds { get; set; }
        public int WatchedSeconds { get; set; }

        public Movie Movie { get; set; }
        public User User { get; set; }
        public History() { }

        public History(Guid movieID, int userID) : this()
        {
            MovieID = movieID;
            UserID = userID;
        }
    }
}
