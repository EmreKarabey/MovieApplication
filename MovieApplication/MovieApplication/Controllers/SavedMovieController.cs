using Application.Features.SavedMovie.Command.Create;
using Application.Features.SavedMovie.Command.Delete;
using Application.Features.SavedMovie.Command.DeleteIdMovie;
using Application.Features.SavedMovie.Command.Update;
using Application.Features.SavedMovie.Queries.GetById;
using Application.Features.SavedMovie.Queries.GetCategoryName;
using Application.Features.SavedMovie.Queries.GetList;
using Application.Features.SavedMovie.Queries.IsSaved;
using Application.Features.SavedMovie.Queries.MySavedMovie;
using Application.Features.SavedMovie.Queries.SearchSaveMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SavedMovieController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListSavedMovieQuery getListLikedMovieQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListSavedMovieDto> result = await Mediator.Send(getListLikedMovieQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdSavedMovieDto result = await Mediator.Send(new GetByIdSavedMovieQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddSavedMovie")]
        public async Task<IActionResult> AddSavedMovie([FromBody] CreatedSavedMovieCommand createdSavedMovieCommand)
        {
            CreatedSavedMovieResponse result = await Mediator.Send(createdSavedMovieCommand);

            return Ok(result);
        }

        [HttpPut("UpdateSavedMovie")]
        public async Task<IActionResult> UpdateSavedMovie([FromBody] UpdateSavedMovieCommand updateSavedMovieCommand)
        {
            UpdateSavedMovieResponse result = await Mediator.Send(updateSavedMovieCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveSavedMovie")]
        public async Task<IActionResult> RemoveSavedMovie([FromQuery] Guid Id)
        {
            DeletedSavedMovieResponse result = await Mediator.Send(new DeletedSavedMovieCommand { Id = Id });

            return Ok(result);
        }

        [HttpDelete("DeleteIdMovie/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> DeleteIdMovie([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            await Mediator.Send(new DeleteIdMovieCommand { MovieID = MovieId, UserID = UserId });
            return Ok();
        }

        [HttpGet("IsSaved/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> GetMovieLikedAny([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            bool result = await Mediator.Send(new GetMovieIsSavedQuery { MovieID = MovieId, UserID = UserId });

            return Ok(result);
        }

        [HttpGet("MySaved/{userId:int}")]
        public async Task<IActionResult> GetMySavedMovie([FromRoute] int userId, [FromQuery] PageRequest pageRequest)
        {
            GetMySavedMovieQuery getMySavedMovieQuery = new() { pageRequest = pageRequest, UserId = userId };

            GetListResponse<GetMySavedMovieDto> result = await Mediator.Send(getMySavedMovieQuery);

            return Ok(result);

        }

        [HttpGet("GetSavedCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] PageRequest pageRequest, [FromQuery] int UserId, [FromQuery] string? CategoryName = null)
        {
            List<GetSavedCategoryNameDto> result = await Mediator.Send(new GetSavedCategoryNameQuery { pageRequest = pageRequest, CategoryName = CategoryName, UserId = UserId });

            return Ok(result);
        }

        [HttpGet("GetSavedMovieSearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search, [FromQuery] int UserId)
        {
            GetListResponse<GetSearchSaveMovieDto> getListResponse = await Mediator.Send(new GetSearchSaveMovieQuery { pageRequest = pageRequest, Search = Search, UserId = UserId });
            return Ok(getListResponse);
        }
    }
}
