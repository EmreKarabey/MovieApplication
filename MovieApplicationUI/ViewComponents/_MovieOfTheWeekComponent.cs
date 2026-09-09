using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Controllers;
using MovieApplicationUI.Dto.Movie;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{

    [ViewComponent(Name = "_MovieOfTheWeekComponent")]
    public class _MovieOfTheWeekComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public _MovieOfTheWeekComponent(IHttpClientFactory httpClientFactory)
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
            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync("api/Movie/8721297b-2df4-4d25-8623-08deb2da08cb");
            var response2 = await client.GetAsync("api/MovieCategory/GetMovie/8721297b-2df4-4d25-8623-08deb2da08cb");

            if (!response.IsSuccessStatusCode || !response2.IsSuccessStatusCode) return Content(string.Empty);

            var json = await response.Content.ReadAsStringAsync();
            var json2 = await response2.Content.ReadAsStringAsync();

            var entity = JsonConvert.DeserializeObject<MovieOfTheWeekDto>(json);
            var entity2 = JsonConvert.DeserializeObject<GetMovieCategoryDto>(json2);

            var result = new MovieCategoryDetail
            {
                EntityId = entity.EntityID,
                CategoryName = entity2.CategoryName,
                Description = entity.Description,
                ImageURL = entity.ImageURL,
                Name = entity.Name,
                ProducerName = entity.ProducerName,
                MovieId = entity.EntityID,
                MovieName = entity.Name,
                VideoURL = entity.VideoURL,
                ReleaseDate = entity.ReleaseDate
            };

            return View(result);
        }
    }
}
