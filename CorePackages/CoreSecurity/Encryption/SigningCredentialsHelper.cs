using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace CoreSecurity.Encryption
{
    public static class SigningCredentialsHelper
    {
        public static SigningCredentials SigningCredentials(SecurityKey securityKey)=>new(securityKey,SecurityAlgorithms.HmacSha256Signature);
    }
}
