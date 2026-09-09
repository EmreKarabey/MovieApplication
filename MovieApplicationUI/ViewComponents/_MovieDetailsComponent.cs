using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Movie;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _MovieDetailsComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MovieDetailsComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid Id)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/MovieCategory/GetMovie/{Id}");

            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<MovieCategoryDetail>(jsonfile);

            return View(file);
        }
    }
}
