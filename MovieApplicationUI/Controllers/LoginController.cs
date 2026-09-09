using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Login;
using MovieApplicationUI.Dto.Register;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index([FromBody] LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");

            var response = await client.PostAsJsonAsync("api/Login/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result.Requires2FA)
                {
                    return Json(new { success = true, requires2FA = true });
                }

                HttpContext.Session.SetString("JWTToken", result.Token);
                return Json(new
                {
                    success = true,
                    redirectUrl = "/Home/Index"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Email veya şifre yanlış"
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");

            var response = await client.PostAsJsonAsync("api/Login/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = string.IsNullOrEmpty(errorMsg) ? "Kayıt işlemi başarısız oldu." : errorMsg });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Verify2FALogin([FromBody] Verify2FADto verifyDto)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");

            var command = new
            {
                Email = verifyDto.Email,
                Code = verifyDto.Code
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Login/TwoFactorAuthenticationLogin", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                HttpContext.Session.SetString("JWTToken", result.Token);
                return Json(new
                {
                    success = true,
                    redirectUrl = "/Home/Index"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Email veya şifre yanlış"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDto)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");

            var response = await client.PostAsJsonAsync("api/Login/GoogleLogin", googleLoginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                HttpContext.Session.SetString("JWTToken", result.Token);
                return Json(new
                {
                    success = true,
                    redirectUrl = "/Home/Index"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Google hesabıyla giriş yapılamadı."
                });
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Index", "Login");
        }
    }
}
