using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;

namespace CoreSecurity.Entities
{
    public class EMailAuthenticator:Entity<int>
    {
        public int UserId { get; set; }
        public string? ActivationKey { get; set; }
        public bool IsVerifed { get; set; }
        public virtual User User { get; set; } = null!;

        public EMailAuthenticator() { }

        public EMailAuthenticator(int userId, string? activationKey, bool ısVerifed)
        {
            UserId = userId;
            ActivationKey = activationKey;
            IsVerifed = ısVerifed;
        }

        public EMailAuthenticator(int id, int userId, string? activationKey, bool ısVerifed) : base(id)
        {
            UserId = userId;
            ActivationKey = activationKey;
            IsVerifed = ısVerifed;
        }
    }
}
