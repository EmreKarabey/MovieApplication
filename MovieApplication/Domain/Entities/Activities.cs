using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Activities : Entity<Guid>
    {
        public int UserID { get; set; }

        public Guid MovieID { get; set; }

        public ActivitiesCategory ActivitiesCategory { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }

        public Activities() { }

        public Activities(int userID, Guid movieID, ActivitiesCategory activitiesCategory)
        {
            UserID = userID;
            MovieID = movieID;
            ActivitiesCategory = activitiesCategory;
        }


    }
    public enum ActivitiesCategory
    {
        Watch = 1,
        Save = 2,
        Favorite = 3
    }

}
