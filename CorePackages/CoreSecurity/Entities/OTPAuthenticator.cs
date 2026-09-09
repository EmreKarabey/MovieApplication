using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;

namespace CoreSecurity.Entities
{
    public class OTPAuthenticator:Entity<int>
    {
        public int UserId { get; set; }
        public byte[] SecretKey { get; set; }
        public bool IsVerifed { get; set; }

        public virtual User User { get; set; } = null!;

        public OTPAuthenticator(int userId, byte[] secretKey, bool isVerifed)
        {
            UserId = userId;
            SecretKey = secretKey;
            IsVerifed = isVerifed;
        }

        public OTPAuthenticator(int id, int userId, byte[] secretKey, bool isVerifed) : base(id)
        {
            UserId = userId;
            SecretKey = secretKey;
            IsVerifed = isVerifed;
        }
    }
}
