using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MovieApplicationUI.Dto.Comment;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _CommentsListComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _CommentsListComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid MovieId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return Content(string.Empty);
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());
            var response = await client.GetAsync($"api/Comment/GetByMovieId/{MovieId}?PageIndex=0&PageSize=10");


            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            ViewBag.VUserName = GetUserName();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PaginateList<CommentListDto>>(json);

            return View(result);
        }
    }
}
