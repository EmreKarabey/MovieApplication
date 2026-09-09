using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Notification;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var responseMessage = await client.GetAsync($"api/Notification/MyNotificationList?UserId={GetUserId()}");

            if (!responseMessage.IsSuccessStatusCode) return View("Error");

            var jsonfile = await responseMessage.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<List<MyNotificationListDto>>(jsonfile);

            return View(file);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsJson()
        {
            if (!IsAuthenticated())
                return Json(new { success = false });

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var responseMessage = await client.GetAsync($"api/Notification/MyNotificationList?UserId={GetUserId()}");

            if (!responseMessage.IsSuccessStatusCode)
                return Json(new { success = false });

            var jsonfile = await responseMessage.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<MyNotificationListDto>>(jsonfile);

            return Json(new { success = true, data = notifications });
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SaveFCMToken([FromBody] TokenRequest request)
        {
            if (!IsAuthenticated() || string.IsNullOrEmpty(request?.Token))
                return Json(new { success = false });

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var payload = new
            {
                UserId = GetUserId(),
                Token = request.Token
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Notification/SaveToken", content);

            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPatch]
        public async Task<IActionResult> Read([FromBody] Guid EntityId)
        {
            if (!IsAuthenticated())
                return Json(new { success = false });

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var command = new
            {
                EntityId = EntityId
            };

            var jsonFile = JsonConvert.SerializeObject(command);

            var stringContent = new StringContent(jsonFile, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PatchAsync("api/Notification/Read", stringContent);

            return Json(new { success = response.IsSuccessStatusCode });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveNotification([FromBody] Guid EntityId)
        {
            if (!IsAuthenticated())
                return Json(new { success = false });

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/Notification/{EntityId}");

            return Json(new { success = response.IsSuccessStatusCode });
        }
    }
}
