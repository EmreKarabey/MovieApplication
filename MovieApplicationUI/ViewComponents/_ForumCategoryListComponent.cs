using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MovieApplicationUI.Dto.ForumCategory;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _ForumCategoryListComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _ForumCategoryListComponent(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync($"api/ForumCategory?PageIndex=0&PageSize=100");

            if (!response.IsSuccessStatusCode) return View();

            var jsonfile = await response.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<PaginateList<ForumCategoryListDto>>(jsonfile);

            return View(file);
        }
    }
}
