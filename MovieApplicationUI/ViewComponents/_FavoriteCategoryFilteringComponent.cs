using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Movie;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _FavoriteCategoryFilteringComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _FavoriteCategoryFilteringComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? CategoryName, int pageIndex = 0)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return Content(string.Empty);
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/FavoriteMovie/GetFavoriteCategory?PageIndex={pageIndex}&PageSize=10&UserId={GetUserId()}&CategoryName={CategoryName}");

            if (!string.IsNullOrEmpty(CategoryName)) response = await client.GetAsync($"api/FavoriteMovie/GetFavoriteCategory?PageIndex={pageIndex}&PageSize=10&UserId={GetUserId()}&CategoryName={CategoryName}");

            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            var json = await response.Content.ReadAsStringAsync();

            var entity = JsonConvert.DeserializeObject<List<GetCatgoryFiltreDto>>(json);

            ViewBag.UserID = GetUserId();

            return View(entity);
        }
    }
}
