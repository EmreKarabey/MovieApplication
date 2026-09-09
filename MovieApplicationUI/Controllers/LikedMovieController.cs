using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.FavoriteMovie;
using MovieApplicationUI.Dto.LikedMovie;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class LikedMovieController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LikedMovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLikedMovie(Guid likedMovieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/LikedMovie/RemoveLikedMovie?Id={likedMovieId}");

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreLikedMovies(int pageIndex)
        {
            if (!IsAuthenticated()) return PartialView("_ErrorPartial");

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/LikedMovie/MyLikedMovie?UserId={GetUserId()}&PageIndex={pageIndex}&PageSize=10");

            if (!response.IsSuccessStatusCode) return PartialView("~/Views/History/_LikedMovieItems.cshtml", new PaginateList<MyLikedMovieDto> { Items = new List<MyLikedMovieDto>() });

            var jsonfile = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PaginateList<MyLikedMovieDto>>(jsonfile);

            return PartialView("~/Views/History/_LikedMovieItems.cshtml", result);
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

            var response = await client.GetAsync($"api/LikedMovie/GetLikedMovieSearch?PageIndex={pageIndex}&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}&UserId={GetUserId()}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<MyLikedMovieDto>>(jsonfile);

            return PartialView("~/Views/History/_LikedMovieItems.cshtml",
                file ?? new PaginateList<MyLikedMovieDto> { Items = new List<MyLikedMovieDto>() });
        }
    }
}
