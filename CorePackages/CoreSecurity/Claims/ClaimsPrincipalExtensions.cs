using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreSecurity.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal,string ClaimTypes)
        {
            var results = claimsPrincipal?.FindAll(ClaimTypes)?.Select(n => n.Value).ToList();

            return results;
        }

        public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Role);

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
            Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());
    }
}
