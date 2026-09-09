using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Announcement;
using Newtonsoft.Json;

namespace MovieApplicationUI.ViewComponents
{
    public class _Editor_sChoiceComponent : _BaseComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _Editor_sChoiceComponent(IHttpClientFactory httpClientFactory)
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

            var response = await client.GetAsync("api/Announcement");

            var json = await response.Content.ReadAsStringAsync();

            var entity = JsonConvert.DeserializeObject<List<AnnouncementListDto>>(json);

            return View(entity);
        }
    }
}
