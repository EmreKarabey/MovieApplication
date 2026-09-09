using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSecurity.OTPAuthenticator
{
    public interface IOTPAuthenticator
    {
        public Task<byte[]> GenerateSecretKey();
        public Task<string> ConvertSecretKeyToString(byte[] secretkey);
        public Task<byte[]> ConvertStringToSecretKey(string base32String);
        public Task<bool> VerfiyCode(byte[] secretKey, string code);
    }
}
