using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
namespace MovieApplicationUI.ViewComponents
{

    public abstract class _BaseComponent : ViewComponent
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
    }
}