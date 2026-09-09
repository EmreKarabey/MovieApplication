using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Movie;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _AccountSettingsComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _AccountSettingsComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return Content(string.Empty);
            }

            string email = GetUserEmail();
            ViewBag.Email = email;

            try
            {
                var client = _httpClientFactory.CreateClient("MovieApi");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

                var response = await client.GetAsync($"api/Login/TwoFactorStatus/{email}");
                if (response.IsSuccessStatusCode)
                {
                    var resultStr = await response.Content.ReadAsStringAsync();
                    ViewBag.Is2FAEnabled = bool.Parse(resultStr);
                }
                else
                {
                    ViewBag.Is2FAEnabled = false;
                }
            }
            catch
            {
                ViewBag.Is2FAEnabled = false;
            }

            return View();
        }
    }
}
