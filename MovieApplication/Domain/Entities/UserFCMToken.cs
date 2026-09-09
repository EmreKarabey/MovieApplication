using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class UserFCMToken : Entity<Guid>
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
