using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class MoviesCategory : Entity<Guid>
    {
        public Guid MovieID { get; set; }
        public Guid CategoryID { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Category Category { get; set; }


        public MoviesCategory() { }


        public MoviesCategory(Guid Id, Guid movieID, Guid categoryID) : this()
        {
            EntityID = Id;
            MovieID = movieID;
            CategoryID = categoryID;
        }
    }
}
