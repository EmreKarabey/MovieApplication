using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Category;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _CategoryListComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _CategoryListComponent(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync("api/Category?PageIndex=0&PageSize=100");

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ApiResponse<CategoryListDto>>(json);

            return View(result.Items);
        }
    }
}
