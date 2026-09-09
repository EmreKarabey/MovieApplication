using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.FavoriteMovie;
using MovieApplicationUI.Dto.SavedMovie;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class FavoriteMovieController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FavoriteMovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? categoryName = null)
        {
            ViewBag.FCategoryName = categoryName;
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var responseTask = client.GetAsync($"api/FavoriteMovie/MyFavorite/{GetUserId()}?PageIndex=0&PageSize=10");
            var response2Task = client.GetAsync($"api/FavoriteMovie/IsFavorited/b6e38e4f-2bc2-4aa8-dfb3-08dec86d8639/{GetUserId()}");

            await Task.WhenAll(responseTask, response2Task);

            var response = await responseTask;
            var response2 = await response2Task;

            if (!response.IsSuccessStatusCode && !response2.IsSuccessStatusCode) return View("Error");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var jsonfile2 = await response2.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<FavoriteMovieListDto>>(jsonfile);
            ViewBag.IsFavorite = JsonConvert.DeserializeObject<bool>(jsonfile2);
            ViewBag.UserId = GetUserId();
            return View(file);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFavoriteMovie(Guid movieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/FavoriteMovie/RemoveByMovieId/{movieId}/{GetUserId()}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest($"Backend Status: {response.StatusCode}. Backend Error: {error}");
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoriteMovie(Guid MovieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var jsonfile = JsonConvert.SerializeObject(new { MovieID = MovieId });

            var stringcontent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/FavoriteMovie/AddFavoriteMovie", stringcontent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            var activityCommand = new { MovieID = MovieId, ActivitiesCategory = 3 };
            var activityContent = new StringContent(JsonConvert.SerializeObject(activityCommand), System.Text.Encoding.UTF8, "application/json");
            await client.PostAsync("api/Activities/AddActivity", activityContent);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> IsMovieFavorite(Guid movieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/FavoriteMovie/IsFavorited/{movieId}/{GetUserId()}");
            if (!response.IsSuccessStatusCode) return Json(false);

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<bool>(jsonfile);

            return Json(file);
        }

        [HttpGet]
        public IActionResult FLoadMoreMovies(string? categoryName, int pageIndex)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            return ViewComponent("_FavoriteCategoryFilteringComponent", new { CategoryName = categoryName, pageIndex = pageIndex });
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

            var response = await client.GetAsync($"api/FavoriteMovie/GetFavoriteMovieSearch?PageIndex={pageIndex}&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}&UserId={GetUserId()}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<FavoriteMovieListDto>>(jsonfile);

            var mapped = file?.Items?.Select(x => new MovieApplicationUI.Dto.Movie.GetCatgoryFiltreDto
            {
                EntityID = x.EntityID,
                Name = x.Name,
                ImageURL = x.ImageURL,
                VideoURL = x.VideoURL,
                Description = x.Description,
                ProducerName = x.ProducerName,
                ReleaseDate = x.ReleaseDate
            }).ToList() ?? new List<MovieApplicationUI.Dto.Movie.GetCatgoryFiltreDto>();

            ViewBag.UserID = GetUserId();

            return PartialView(
                "~/Views/Shared/Components/_FavoriteCategoryFilteringComponent/Default.cshtml",
                mapped
            );
        }
    }
}
