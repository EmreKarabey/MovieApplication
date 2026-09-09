using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace MovieApplicationUI.ViewComponents
{
    public class _SidebarComponent : _BaseComponent
    {
        public IViewComponentResult Invoke()
        {
            var name = GetUserName();
            ViewBag.Name = name;
            return View();
        }
    }
}
