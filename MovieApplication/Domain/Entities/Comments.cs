using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Comments : Entity<Guid>
    {


        public string Content { get; set; }

        public int UserID { get; set; }
        public Guid MovieID { get; set; }


        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }

        public ICollection<SubComment> SubComments { get; set; }


        public Comments() { }

        public Comments(Guid Id, string content, int userID, Guid movieID) : this()
        {
            EntityID = Id;
            Content = content;
            UserID = userID;
            MovieID = movieID;
        }
    }
}
