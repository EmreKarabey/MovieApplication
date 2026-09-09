using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.FavoriteMovie;
using MovieApplicationUI.Dto.SavedMovie;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class SavedMovieController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SavedMovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? categoryName = null)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/SavedMovie/MySaved/{GetUserId()}?PageIndex=0&PageSize=10");

            if (!response.IsSuccessStatusCode) return View("Error");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<SavedMovieListDto>>(jsonfile);

            return View(file);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSavedMovie(Guid movieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync($"api/SavedMovie/DeleteIdMovie/{movieId}/{GetUserId()}");

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddSavedMovie(Guid MovieId)
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

            var response = await client.PostAsync($"api/SavedMovie/AddSavedMovie", stringcontent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            var activityCommand = new { MovieID = MovieId, ActivitiesCategory = 2 };
            var activityContent = new StringContent(JsonConvert.SerializeObject(activityCommand), System.Text.Encoding.UTF8, "application/json");
            await client.PostAsync("api/Activities/AddActivity", activityContent);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> IsMovieSaved(Guid movieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/SavedMovie/IsSaved/{movieId}/{GetUserId()}");
            if (!response.IsSuccessStatusCode) return Json(false);

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<bool>(jsonfile);

            return Json(file);
        }

        [HttpGet]
        public IActionResult SLoadMoreMovies(string? categoryName, int pageIndex)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            return ViewComponent("_SavedCategoryFilteringComponent", new { CategoryName = categoryName, pageIndex = pageIndex });
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

            var response = await client.GetAsync($"api/SavedMovie/GetSavedMovieSearch?PageIndex={pageIndex}&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}&UserId={GetUserId()}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<SavedMovieListDto>>(jsonfile);

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
                "~/Views/Shared/Components/_SavedCategoryFilteringComponent/Default.cshtml",
                mapped
            );
        }

    }
}
