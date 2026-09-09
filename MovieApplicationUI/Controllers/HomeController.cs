using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Models;

namespace MovieApplicationUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index(string? categoryName = null)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            ViewBag.CategoryName = categoryName;
            ViewBag.UserID = GetUserId();

            var roles = GetRole();

            if (roles.Any(r => r.Equals("Publisher", StringComparison.OrdinalIgnoreCase)))
                ViewBag.Role = true;
            else
                ViewBag.Role = false;

            return View();
        }

        [HttpGet]
        public IActionResult LoadMoreMovies(string? categoryName, int pageIndex)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            return ViewComponent("_CategoryFilteringComponent", new { CategoryName = categoryName, pageIndex = pageIndex });
        }



        public IActionResult Privacy()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
