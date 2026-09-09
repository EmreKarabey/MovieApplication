using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Comment;
using MovieApplicationUI.Dto.Forum;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ForumController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var list = await client.GetAsync("api/Forum/ActiveList?PageIndex=0&PageSize=10");

            var jsonfile = await list.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<PaginateList<GetActiveForumListDto>>(jsonfile);

            return View(file);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var list = await client.GetAsync($"api/Forum/{id}");

            var jsonfile = await list.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<GetForumDetailsDto>(jsonfile);

            return View(file);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddForumDto addForumDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(addForumDto);

            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var result = await client.PostAsync("api/Forum", stringContent);

            if (!result.IsSuccessStatusCode) return View(addForumDto);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetMoreForums(int pageIndex)
        {
            if (!IsAuthenticated()) return Unauthorized();

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var list = await client.GetAsync($"api/Forum/ActiveList?PageIndex={pageIndex}&PageSize=10");
            var jsonfile = await list.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<GetActiveForumListDto>>(jsonfile);

            return Json(file);
        }
    }
}
