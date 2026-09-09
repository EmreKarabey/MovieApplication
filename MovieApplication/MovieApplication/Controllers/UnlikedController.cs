using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.UnlikeMovie;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Queries.GetMovieLikedCount;
using Application.Features.LikedMovie.Queries.MyLikedMovie;
using Application.Features.LikedMovie.Queries.SearchLikedMovie;
using Application.Features.UnlikedMovie.Command.Delete;
using Application.Features.UnlikedMovie.Commands.Create;
using Application.Features.UnlikedMovie.Commands.UnlikedMovie;
using Application.Features.UnlikedMovie.Commands.Update;
using Application.Features.UnlikedMovie.Queries.GetById;
using Application.Features.UnlikedMovie.Queries.GetList;
using Application.Features.UnlikedMovie.Queries.GetMovieLikedCount;
using Application.Features.UnlikedMovie.Queries.IsLiked;
using Application.Features.UnlikedMovie.Queries.MyUnlikedMovies;
using Application.Features.UnlikedMovie.Queries.SearchUnlikedMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [Authorize]
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UnlikedController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListUnlikedMovieQuery getListUnlikedMovieQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListUnlikedMovieDto> result = await Mediator.Send(getListUnlikedMovieQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdUnlikedMovieDto result = await Mediator.Send(new GetByIdUnlikedMovieQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddUnlikedMovie")]
        public async Task<IActionResult> AddUnlikedMovie([FromBody] CreatedUnlikedMovieCommand createdUnlikedMovieCommand)
        {
            CreatedUnlikedMovieResponse result = await Mediator.Send(createdUnlikedMovieCommand);

            return Ok(result);
        }

        [HttpPut("UpdateUnlikedMovie")]
        public async Task<IActionResult> UpdateUnlikedMovie([FromBody] UpdateUnlikedMovieCommand updateUnlikedMovieCommand)
        {
            UpdateUnlikedMovieResponse result = await Mediator.Send(updateUnlikedMovieCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveUnlikedMovie")]
        public async Task<IActionResult> RemoveUnlikedMovie([FromQuery] Guid Id)
        {
            DeletedUnlikedMovieResponse result = await Mediator.Send(new DeletedUnlikedMovieCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("MovieUnlikedCount/{Id:guid}")]
        public async Task<IActionResult> GetMovieUnlikedCount([FromRoute] Guid Id)
        {
            int result = await Mediator.Send(new GetMovieUnlikedCountQuery { MovieID = Id });

            return Ok(result);
        }

        [HttpGet("IsUnLiked/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> GetMovieUnlikedAny([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            bool result = await Mediator.Send(new GetMovieIsUnlikedQuery { MovieId = MovieId, UserId = UserId });

            return Ok(result);
        }

        [HttpDelete("UnlikedMovie/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> UnlikeMovie([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            await Mediator.Send(new UnlikeMovieCommand { MovieId = MovieId, UserId = UserId });
            return Ok();
        }

        [HttpGet("MyUnlikedMovie")]
        public async Task<IActionResult> MyUnlikedMovie([FromQuery] int UserId, [FromQuery] PageRequest pageRequest)
        {
            GetMyUnlikedMovieQuery getMyUnlikedMovieQuery = new() { UserId = UserId, pageRequest = pageRequest };

            GetListResponse<GetMyUnlikedMovieDto> result = await Mediator.Send(getMyUnlikedMovieQuery);

            return Ok(result);
        }

        [HttpGet("GetUnlikedMovieSearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search, [FromQuery] int UserId)
        {
            GetListResponse<GetSearchUnlikedMovieDto> getListResponse = await Mediator.Send(new GetSearchUnlikedMovieQuery { pageRequest = pageRequest, Search = Search, UserId = UserId });
            return Ok(getListResponse);
        }
    }
}
