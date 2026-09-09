using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class SubComment : Entity<Guid>
    {
        public string Content { get; set; }

        public int UserID { get; set; }
        public Guid CommentID { get; set; }


        public virtual User User { get; set; }
        public virtual Comments Comments { get; set; }


        public SubComment() { }

        public SubComment(Guid Id, string content, int userID, Guid commentID) : this()
        {
            EntityID = Id;
            Content = content;
            UserID = userID;
            CommentID = commentID;
        }
    }
}
