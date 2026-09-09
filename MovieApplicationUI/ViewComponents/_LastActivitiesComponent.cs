using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Activities;
using MovieApplicationUI.Dto.Comment;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;


namespace MovieApplicationUI.ViewComponents
{
    public class _LastActivitiesComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _LastActivitiesComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());
            var response = await client.GetAsync($"api/Activities/MyActivities?UserID={GetUserId()}&PageIndex=0&PageSize=4");


            if (!response.IsSuccessStatusCode) return Content(string.Empty);

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PaginateList<MyActivityDto>>(json);

            return View(result);
        }


    }
}


