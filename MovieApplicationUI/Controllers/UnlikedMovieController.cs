using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.LikedMovie;
using MovieApplicationUI.Dto.UnlikedMovie;
using MovieApplicationUI.Paginate;
using MovieApplicationUI.ViewComponents;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class UnlikedMovieController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UnlikedMovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUnlikedMovie(Guid unlikedMovieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/Unliked/RemoveUnlikedMovie?Id={unlikedMovieId}");

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreUnlikedMovies(int pageIndex)
        {
            if (!IsAuthenticated()) return PartialView("_ErrorPartial");

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/Unliked/MyUnlikedMovie?UserId={GetUserId()}&PageIndex={pageIndex}&PageSize=10");

            if (!response.IsSuccessStatusCode) return PartialView("~/Views/History/_UnlikedMovieItems.cshtml", new PaginateList<MyUnlikedMovieDto> { Items = new List<MyUnlikedMovieDto>() });

            var jsonfile = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaginateList<MyUnlikedMovieDto>>(jsonfile);

            return PartialView("~/Views/History/_UnlikedMovieItems.cshtml", result);
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

            var response = await client.GetAsync($"api/Unliked/GetUnlikedMovieSearch?PageIndex={pageIndex}&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}&UserId={GetUserId()}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<MyUnlikedMovieDto>>(jsonfile);

            return PartialView("~/Views/History/_UnlikedMovieItems.cshtml",
                file ?? new PaginateList<MyUnlikedMovieDto> { Items = new List<MyUnlikedMovieDto>() });
        }
    }
}
