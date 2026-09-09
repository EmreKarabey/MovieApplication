using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Notification : Entity<Guid>
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public bool IsRead { get; set; }
    }
}
