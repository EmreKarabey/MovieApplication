using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreSecurity.Hashing
{
    public class HashingKeyHelper
    {
        public static void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using HMACSHA512 hmac = new HMACSHA512();

            PasswordSalt = hmac.Key;
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
        }

        public static bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using HMACSHA512 hmac = new(PasswordSalt);

            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));

            return computedHash.SequenceEqual(PasswordHash);
        }
    }
}
