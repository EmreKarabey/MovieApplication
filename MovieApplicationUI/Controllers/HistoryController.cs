using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.FavoriteMovie;
using MovieApplicationUI.Dto.History;
using MovieApplicationUI.Dto.SavedMovie;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class HistoryController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HistoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/History/MyHistory/{GetUserId()}?PageIndex=0&PageSize=10");

            if (!response.IsSuccessStatusCode) return View("Error");
            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<HistoryListDto>>(jsonfile);
            return View(file);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveHistory(Guid historyId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/History/RemoveHistory?Id={historyId}");

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreHistory(int pageIndex)
        {
            if (!IsAuthenticated()) return PartialView("_ErrorPartial");

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/History/MyHistory/{GetUserId()}?PageIndex={pageIndex}&PageSize=10");

            if (!response.IsSuccessStatusCode) return PartialView("~/Views/History/_HistoryItems.cshtml", new PaginateList<HistoryListDto> { Items = new List<HistoryListDto>() });

            var jsonfile = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaginateList<HistoryListDto>>(jsonfile);

            return PartialView("~/Views/History/_HistoryItems.cshtml", result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string s, int pageIndex = 0)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/History/GetHistorySearch?PageIndex={pageIndex}&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}&UserId={GetUserId()}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<HistoryListDto>>(jsonfile);

            return PartialView("~/Views/History/_HistoryItems.cshtml", file ?? new PaginateList<HistoryListDto> { Items = new List<HistoryListDto>() });
        }
    }
}
