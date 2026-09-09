using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace CoreSecurity.Encryption
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey SecurityKey(string securitKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitKey));
    }
}
