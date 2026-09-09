using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MovieApplicationUI.Dto.Account;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettingsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SendEmailCode()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                Email = GetUserEmail()
            };

            var jsonfile = JsonConvert.SerializeObject(command);

            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Login/SendCode", stringContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpPost]

        public async Task<IActionResult> UpdateEmailCode(string email, int code)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                Email = GetUserEmail(),
                NewEmail = email,
                Code = code
            };

            var jsonfile = JsonConvert.SerializeObject(command);

            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/Login/UpdateEmail", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMsg);
            }

            var resultString = await response.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<UpdateTokenDto>(resultString);

            string newToken = file?.Token;
            if (!string.IsNullOrEmpty(newToken))
            {
                HttpContext.Session.SetString("JWTToken", newToken);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(int code, string password, string confirmPassword)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                Email = GetUserEmail(),
                Code = code,
                NewPassword = password,
                ConfirmPassword = confirmPassword
            };

            var jsonfile = JsonConvert.SerializeObject(command);

            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/Login/ResetPassword", stringContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMsg);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TwoFactorAuthentication(string? code, string? secretKey)
        {
            if (!IsAuthenticated())
            {
                return Json(new { isSuccess = false, message = "Lütfen giriş yapın." });
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                Email = GetUserEmail(),
                Code = code,
                SecretKey = secretKey
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Login/TwoFactorAuthentication", jsonContent);

            if (!response.IsSuccessStatusCode) { return BadRequest(); }

            var resultString = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<string>(resultString);

            if (string.IsNullOrEmpty(code))
            {
                string userEmail = GetUserEmail() ?? "kullanici";
                return Json(new { isSuccess = true, secretKey = apiResult, email = userEmail });
            }
            else
            {
                if (apiResult == "Success")
                    return Json(new { isSuccess = true, message = "İki adımlı doğrulama başarıyla aktifleştirildi!" });
                else
                    return Json(new { isSuccess = false, message = "Hatalı kod girdiniz." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            if (!IsAuthenticated())
            {
                return Json(new { isSuccess = false, message = "Lütfen giriş yapın." });
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                Email = GetUserEmail()
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Login/DisableTwoFactorAuthentication", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { isSuccess = false, message = "İşlem sırasında bir hata oluştu." });
            }

            return Json(new { isSuccess = true, message = "İki adımlı doğrulama başarıyla kapatıldı!" });
        }

        [HttpPost]
        public async Task<IActionResult> ActiveDarkMode()
        {
            if (!IsAuthenticated())
            {
                return Json(new { isSuccess = false, message = "Lütfen giriş yapın." });
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                UserId = GetUserId()
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/DarkMode/ActiveDarkMode", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { isSuccess = false, message = "İşlem sırasında bir hata oluştu." });
            }

            return Json(new { isSuccess = true, message = "Karanlık Mod başarıyla aktif!" });
        }

        [HttpPost]
        public async Task<IActionResult> DisableDarkMode()
        {
            if (!IsAuthenticated())
            {
                return Json(new { isSuccess = false, message = "Lütfen giriş yapın." });
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                UserId = GetUserId()
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/DarkMode/DisableDarkMode", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { isSuccess = false, message = "İşlem sırasında bir hata oluştu." });
            }

            return Json(new { isSuccess = true, message = "Karanlık Mod başarıyla kapatıldı!" });
        }


        [HttpGet]
        public async Task<IActionResult> IsDarkMode()
        {
            if (!IsAuthenticated())
            {
                return Json(new { isSuccess = false, message = "Lütfen giriş yapın." });
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/DarkMode/IsDarkMode/{GetUserId()}");

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { isSuccess = false, message = "İşlem sırasında bir hata oluştu." });
            }

            var resultString = await response.Content.ReadAsStringAsync();
            bool isDark = false;
            bool.TryParse(resultString.Trim().Trim('"'), out isDark);

            return Json(new { isSuccess = isDark });
        }

    }
}
