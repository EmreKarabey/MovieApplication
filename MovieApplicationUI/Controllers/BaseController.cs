using System.IdentityModel.Tokens.Jwt;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;

namespace MovieApplicationUI.Controllers
{

    public class BaseController : Controller
    {
        protected string GetToken() => HttpContext.Session.GetString("JWTToken");

        protected bool IsAuthenticated() => !string.IsNullOrEmpty(GetToken());

        protected int GetUserId()
        {
            var token = GetToken();

            if (string.IsNullOrEmpty(token)) return 0;

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            return Convert.ToInt32(jwt.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "nameid")?.Value);
        }

        protected string GetUserEmail()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }

        protected string GetUserName()
        {
            var token = GetToken();

            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.FirstOrDefault(c => c.Type == "unique_name" || c.Type == "name")?.Value;
        }

        protected List<string> GetRole()
        {
            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var roles = jwt.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(c => c.Value).ToList();

                return roles;
            }

            return new List<string>();
        }
    }
}
