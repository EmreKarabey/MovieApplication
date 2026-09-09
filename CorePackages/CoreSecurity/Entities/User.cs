using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CorePersistence.Repositories;
using CoreSecurity.Enums;

namespace CoreSecurity.Entities
{
    public class User : Entity<int>
    {


        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EMail { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public string AccountType { get; set; } = "member";
        public bool Status { get; set; }

        public int? Code { get; set; }
        public DateTime? CodeDuration { get; set; }
        public bool TwoFactor { get; set; } = false;

        public bool DarkMode { get; set; } = false;


        public virtual ICollection<AuthenticatorType> AuthenticatorTypes { get; set; } = new List<AuthenticatorType>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = new List<UserOperationClaim>();
        public virtual ICollection<EMailAuthenticator> EMailAuthenticators { get; set; } = new List<EMailAuthenticator>();
        public virtual ICollection<OTPAuthenticator> OTPAuthenticators { get; set; } = new List<OTPAuthenticator>();


        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PasswordSalt = Array.Empty<byte>();
            PasswordHash = Array.Empty<byte>();
        }


        public User(string firstName, string lastName, byte[] passwordSalt, byte[] passwordHash, bool status)
        {
            FirstName = firstName;
            LastName = lastName;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Status = status;
        }

        public User(int id, string firstName, string lastName, byte[] passwordSalt, byte[] passwordHash, bool status) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Status = status;
        }
    }
}
