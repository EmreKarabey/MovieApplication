using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CoreSecurity.Claims
{
    public static class ClaimExtensions
    {
        public static void AddEMail(this ICollection<Claim> claims, string email)
            => claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

        public static void AddName(this ICollection<Claim> claims, string name)
            => claims.Add(new Claim(JwtRegisteredClaimNames.Name, name));

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
            => claims.Add(new Claim(JwtRegisteredClaimNames.NameId, nameIdentifier));

        public static void AddRole(this ICollection<Claim> claims, string[] Roles)
            => Roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
    }
}
