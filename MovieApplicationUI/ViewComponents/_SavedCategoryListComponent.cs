using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Category;
using MovieApplicationUI.Dto.Movie;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _SavedCategoryListComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _SavedCategoryListComponent(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync($"api/Category?PageIndex=0&PageSize=100");

            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<CategoryListDto>>(json);

            ViewBag.UserID = GetUserId();

            return View(result.Items);
        }
    }

}
