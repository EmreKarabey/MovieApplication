using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OtpNet;

namespace CoreSecurity.OTPAuthenticator
{
    public class OTPNetAuthenticatorHelper : IOTPAuthenticator
    {
        public Task<string> ConvertSecretKeyToString(byte[] secretkey)
        {
            string base32String=Base32Encoding.ToString(secretkey);
            return Task.FromResult(base32String);
        }

        public Task<byte[]> ConvertStringToSecretKey(string base32String)
        {
            byte[] bytes = Base32Encoding.ToBytes(base32String);
            return Task.FromResult(bytes);
        }

        public Task<byte[]> GenerateSecretKey()
        {
            byte[] key = KeyGeneration.GenerateRandomKey(20);
            string base32String = Base32Encoding.ToString(key);
            byte[] base32bytes = Base32Encoding.ToBytes(base32String);

            return Task.FromResult(base32bytes);
        }

        public Task<bool> VerfiyCode(byte[] secretKey, string code)
        {
            Totp totp = new Totp(secretKey);

            string Toptcode = totp.ComputeTotp(DateTime.UtcNow);

            bool results = Toptcode == code;

            return Task.FromResult(results);
        }
    }
}
