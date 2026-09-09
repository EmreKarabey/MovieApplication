using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApplicationUI.Dto.Comment;
using MovieApplicationUI.Dto.History;
using MovieApplicationUI.Dto.Movie;
using MovieApplicationUI.Dto.SavedMovie;
using MovieApplicationUI.Dto.SubComment;
using MovieApplicationUI.Dto.Subscription;
using MovieApplicationUI.Paginate;
using Newtonsoft.Json;

namespace MovieApplicationUI.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Play([FromQuery] string videoUrl, [FromQuery] string movieName, [FromQuery] Guid MovieID)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var responseTask = client.GetAsync($"api/LikedMovie/IsLiked/{MovieID}/{GetUserId()}");
            var response2Task = client.GetAsync($"api/LikedMovie/MovieLikedCount/{MovieID}");

            var response3Task = client.GetAsync($"api/Unliked/IsUnLiked/{MovieID}/{GetUserId()}");
            var response4Task = client.GetAsync($"api/Unliked/MovieUnlikedCount/{MovieID}");

            var response5Task = client.GetAsync($"api/SavedMovie/IsSaved/{MovieID}/{GetUserId()}");

            var myCommand = new
            {
                MovieID = MovieID,
                TotalSeconds = 0,
                WatchedSeconds = 0
            };
            var jsonfile = JsonConvert.SerializeObject(myCommand);
            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");
            var response6Task = client.PostAsync("api/History/AddHistory", stringContent);

            var activityCommand = new { MovieID = MovieID, ActivitiesCategory = 1 };
            var activityContent = new StringContent(JsonConvert.SerializeObject(activityCommand), System.Text.Encoding.UTF8, "application/json");
            var activityTask = client.PostAsync("api/Activities/AddActivity", activityContent);

            var response7Task = client.GetAsync($"api/Movie/{MovieID}");

            await Task.WhenAll(responseTask, response2Task, response3Task, response4Task, response5Task, response6Task, activityTask, response7Task);

            var response = await responseTask;
            var response2 = await response2Task;
            var response3 = await response3Task;
            var response4 = await response4Task;
            var response5 = await response5Task;
            var response6 = await response6Task;
            var response7 = await response7Task;

            if (!response.IsSuccessStatusCode || !response2.IsSuccessStatusCode || !response3.IsSuccessStatusCode || !response4.IsSuccessStatusCode || !response5.IsSuccessStatusCode || !response6.IsSuccessStatusCode || !response7.IsSuccessStatusCode) return View("Error");

            var result = await response.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            var result3 = await response3.Content.ReadAsStringAsync();
            var result4 = await response4.Content.ReadAsStringAsync();
            var result5 = await response5.Content.ReadAsStringAsync();
            var result6 = await response6.Content.ReadAsStringAsync();
            var result7 = await response7.Content.ReadAsStringAsync();

            var historyData = JsonConvert.DeserializeObject<CreatedHistoryResponseDto>(result6);
            var publisherData = JsonConvert.DeserializeObject<PublisherDataDto>(result7);

            var response8 = await client.GetAsync($"api/Subscription/IsSubscribe/{publisherData.PublisherId}");
            var result8 = await response8.Content.ReadAsStringAsync();
            var IsSubscribe = JsonConvert.DeserializeObject<bool>(result8);

            ViewBag.HistoryId = historyData?.EntityID;
            ViewBag.WatchedSeconds = historyData?.WatchedSeconds ?? 0;

            ViewBag.IsLiked = JsonConvert.DeserializeObject<bool>(result);
            ViewBag.LikeCount = JsonConvert.DeserializeObject<int>(result2);

            ViewBag.IsDisliked = JsonConvert.DeserializeObject<bool>(result3);
            ViewBag.DislikeCount = JsonConvert.DeserializeObject<int>(result4);

            ViewBag.IsSaved = JsonConvert.DeserializeObject<bool>(result5);

            ViewBag.Token = GetToken();
            ViewBag.MovieName = movieName;
            ViewBag.VideoUrl = videoUrl;
            ViewBag.MovieID = MovieID;
            ViewBag.UserID = GetUserId();
            ViewBag.VUserName = GetUserName();

            ViewBag.PublisherFirstName = publisherData.PublisherFirstName;
            ViewBag.PublisherLastName = publisherData.PublisherLastName;
            ViewBag.PublisherId = publisherData.PublisherId;

            ViewBag.IsSubscribe = IsSubscribe;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress([FromBody] UpdateHistoryProgressDto dto)
        {
            if (!IsAuthenticated()) return Unauthorized();

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/History/UpdateHistory", content);

            return response.IsSuccessStatusCode ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentDto addCommentDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            addCommentDto.UserId = GetUserId();

            var json = JsonConvert.SerializeObject(addCommentDto);

            var encoding = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Comment/AddComment", encoding);

            if (!response.IsSuccessStatusCode) return Json(new { success = false, message = "Yorum eklenirken hata oluştu." });

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreComments(Guid movieId, int pageIndex)
        {
            {
                if (!IsAuthenticated())
                {
                    HttpContext.Response.Redirect("/Login/Index");
                    return new EmptyResult();
                }

                var client = _httpClientFactory.CreateClient("MovieApi");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

                var response = await client.GetAsync($"api/Comment/GetByMovieId/{movieId}?PageIndex={pageIndex}&PageSize=10");

                if (!response.IsSuccessStatusCode) return PartialView("_CommentItems", new Paginate.PaginateList<MovieApplicationUI.Dto.Comment.CommentListDto> { Items = new List<MovieApplicationUI.Dto.Comment.CommentListDto>() });

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Paginate.PaginateList<MovieApplicationUI.Dto.Comment.CommentListDto>>(json);
                return PartialView("~/Views/Shared/Components/_CommentsListComponent/_CommentItems.cshtml", result);
            }
        }

        [HttpGet]
        public IActionResult GetMovieDetailsModal(Guid id)
        {
            return ViewComponent("_MovieDetailsComponent", new { Id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string s, int pageIndex = 0)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync($"api/Movie/GetMovieSearch?PageIndex=0&PageSize=10&Search={Uri.EscapeDataString(s ?? "")}");

            if (!response.IsSuccessStatusCode) return PartialView("_ErrorPartial");

            var jsonfile = await response.Content.ReadAsStringAsync();
            var file = JsonConvert.DeserializeObject<PaginateList<SearchMovieListDto>>(jsonfile);

            var mapped = file?.Items?.Select(x => new MovieApplicationUI.Dto.Movie.GetCatgoryFiltreDto
            {
                EntityID = x.EntityID,
                Name = x.Name,
                ImageURL = x.ImageURL,
                VideoURL = x.VideoURL,
                Description = x.Description,
                ProducerName = x.ProducerName,
                ReleaseDate = x.ReleaseDate
            }).ToList() ?? new List<MovieApplicationUI.Dto.Movie.GetCatgoryFiltreDto>();

            ViewBag.UserID = GetUserId();

            return PartialView(
                "~/Views/Shared/Components/_CategoryFilteringComponent/Default.cshtml",
                mapped
            );
        }


        [HttpGet]
        public IActionResult Publish()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var roles = GetRole();
            if (!roles.Any(r => r.Equals("Publisher", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                HttpContext.Response.Redirect("/Home/Index");
                return new EmptyResult();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Publish(PublishMovieDto publishMovieDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var roles = GetRole();
            if (!roles.Any(r => r.Equals("Publisher", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                HttpContext.Response.Redirect("/Home/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var form = new MultipartFormDataContent();

            form.Add(new StringContent(publishMovieDto.Description), "Description");
            form.Add(new StringContent(publishMovieDto.Name), "Name");
            form.Add(new StringContent(publishMovieDto.ProducerName), "ProducerName");
            form.Add(new StringContent(publishMovieDto.ReleaseDate.ToString()), "ReleaseDate");


            if (publishMovieDto.ImageFile != null && publishMovieDto.VideoFile != null)
            {
                var stream = publishMovieDto.ImageFile.OpenReadStream();
                var stream2 = publishMovieDto.VideoFile.OpenReadStream();

                var filecontent = new StreamContent(stream);
                var filecontent2 = new StreamContent(stream2);

                form.Add(filecontent, "ImageFile", publishMovieDto.ImageFile.FileName);
                form.Add(filecontent2, "VideoFile", publishMovieDto.VideoFile.FileName);
            }

            var responsemessage = await client.PostAsync("api/Movie/AddMovie", form);

            if (!responsemessage.IsSuccessStatusCode) return RedirectToAction("Index", "Home");

            var readResponse = await responsemessage.Content.ReadAsStringAsync();

            var file = JsonConvert.DeserializeObject<MovieDto>(readResponse);

            TempData["MovieID"] = file.EntityID;

            return RedirectToAction("SelectCategory");
        }

        [HttpGet]
        public IActionResult SelectCategory()
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var roles = GetRole();
            if (!roles.Any(r => r.Equals("Publisher", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                HttpContext.Response.Redirect("/Home/Index");
                return new EmptyResult();
            }

            TempData.Keep("MovieID");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectCategory(PublishTableMovieCategoryDto publishMovieCategoryDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var roles = GetRole();
            if (!roles.Any(r => r.Equals("Publisher", StringComparison.OrdinalIgnoreCase) || r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            {
                HttpContext.Response.Redirect("/Home/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            if (TempData["MovieID"] != null)
            {
                publishMovieCategoryDto.MovieID = (Guid)TempData["MovieID"];
            }

            var jsonfile = JsonConvert.SerializeObject(publishMovieCategoryDto);

            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync("api/MovieCategory/AddMovieCategory", stringContent);

            if (!responseMessage.IsSuccessStatusCode) return new EmptyResult();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Subscription([FromBody] MovieApplicationUI.Dto.Subscription.AddSubscriptionDto addSubscriptionDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }
            addSubscriptionDto.UserId = GetUserId();

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var jsonfile = JsonConvert.SerializeObject(addSubscriptionDto);
            var stringContent = new StringContent(jsonfile, System.Text.Encoding.UTF8, "application/json");

            var reponseMessage = await client.PostAsync("api/Subscription", stringContent);

            if (!reponseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Abonelik işlemi başarısız." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Unsubscription([FromBody] int channelId)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var reponseMessage = await client.DeleteAsync($"api/Subscription/Unsubscribe/{channelId}");

            if (!reponseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Abonelikten çıkma işlemi başarısız." });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddSubComment(AddSubCommentDto addSubCommentDto)
        {
            if (!IsAuthenticated())
            {
                HttpContext.Response.Redirect("/Login/Index");
                return new EmptyResult();
            }

            var client = _httpClientFactory.CreateClient("MovieApi");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            addSubCommentDto.UserID = GetUserId();

            var json = JsonConvert.SerializeObject(addSubCommentDto);

            var encoding = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/SubComments/AddSubComment", encoding);

            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Alt yorum eklenirken bir hata oluştu." });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetSubComments(Guid commentId, int pageIndex = 0, int pageSize = 10)
        {
            var client = _httpClientFactory.CreateClient("MovieApi");

            var token = GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"api/SubComments/GetByCommentId/{commentId}?PageIndex={pageIndex}&PageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Paginate.PaginateList<MovieApplicationUI.Dto.SubComment.SubCommentListDto>>(jsonString);
                return PartialView("_SubCommentsList", result.Items);
            }

            return Content("");
        }

    }

    public class MovieDto
    {
        public Guid EntityID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }
    }
}
