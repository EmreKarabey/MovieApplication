using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto;
using MovieApplicationUI.Dto.Comment;
using MovieApplicationUI.Dto.History;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _MyStaticsComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _MyStaticsComponent(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync($"api/History/MyStatics?UserId={GetUserId()}");


            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            ViewBag.VUserName = GetUserName();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<MyStaticsMovieDto>(json);

            return View(result);
        }
    }
}
