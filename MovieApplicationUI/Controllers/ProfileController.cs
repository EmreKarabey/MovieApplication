using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Account;
using MovieApplicationUI.Dto.History;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            ViewBag.Name = GetUserName();
            ViewBag.Email = GetUserEmail();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateAccountDto updateAccountDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            updateAccountDto.UserId = GetUserId();

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(updateAccountDto);

            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var result = await client.PutAsync("api/Login/UpdateAccount", stringContent);

            if (result.IsSuccessStatusCode)
            {
                var responseContent = await result.Content.ReadAsStringAsync();
                var updateResponse = JsonConvert.DeserializeObject<MovieApplicationUI.Dto.Account.UpdateAccountResponseDto>(responseContent);
                if (updateResponse != null && !string.IsNullOrEmpty(updateResponse.Token))
                {
                    HttpContext.Session.SetString("JWTToken", updateResponse.Token);
                }
                return Json(new { success = true, message = "Bilgiler başarıyla güncellendi." });
            }

            return Json(new { success = false, message = "Güncelleme başarısız oldu." });
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreActivities([FromServices] IHttpClientFactory httpClientFactory, int pageIndex)
        {
            if (!IsAuthenticated()) return Unauthorized();

            var client = httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());
            var response = await client.GetAsync($"api/Activities/MyActivities?UserID={GetUserId()}&PageIndex={pageIndex}&PageSize=4");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var json = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieApplicationUI.Paginate.PaginateList<MovieApplicationUI.Dto.Activities.MyActivityDto>>(json);

            if (result == null || result.Items == null || !result.Items.Any())
                return Content("");

            return PartialView("_ActivityItemsPartial", result.Items);
        }
    }
}
