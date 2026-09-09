using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Forum : Entity<Guid>
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public Guid ForumCategoryId { get; set; }
        public ForumCategory ForumCategory { get; set; }

        public string Status { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public Forum() { }

        public Forum(string title, string details, string status, Guid forumCategoryId, int userId) : this()
        {
            Title = title;
            Details = details;
            ForumCategoryId = forumCategoryId;
            UserId = UserId;
            Status = status;
        }
    }
}
