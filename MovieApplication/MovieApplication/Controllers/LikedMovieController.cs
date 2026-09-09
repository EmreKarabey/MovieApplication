using System.IdentityModel.Tokens.Jwt;
using Application.Features.Category.Command.Create;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Queries.GetById;
using Application.Features.Category.Queries.GetList;
using Application.Features.FavoriteMovie.Queries.SearchFavoriteMovie;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.UnlikeMovie;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Queries.GetMovieLikedCount;
using Application.Features.LikedMovie.Queries.IsLiked;
using Application.Features.LikedMovie.Queries.MyLikedMovie;
using Application.Features.LikedMovie.Queries.SearchLikedMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json.Linq;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikedMovieController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListLikedMovieQuery getListLikedMovieQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListLikedMovieDto> result = await Mediator.Send(getListLikedMovieQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdLikedMovieDto result = await Mediator.Send(new GetByIdLikedMovieQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddLikedMovie")]
        public async Task<IActionResult> AddLikedMovie([FromBody] CreatedLikedMovieCommand createdLikedMovieCommand)
        {
            CreatedLikedMovieResponse result = await Mediator.Send(createdLikedMovieCommand);

            return Ok(result);
        }

        [HttpPut("UpdateLikedMovie")]
        public async Task<IActionResult> UpdateLikedMovie([FromBody] UpdateLikedMovieCommand updateLikedMovieCommand)
        {
            UpdateLikedMovieResponse result = await Mediator.Send(updateLikedMovieCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveLikedMovie")]
        public async Task<IActionResult> RemoveLikedMovie([FromQuery] Guid Id)
        {
            DeletedLikedMovieResponse result = await Mediator.Send(new DeletedLikedMovieCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("MovieLikedCount/{Id:guid}")]
        public async Task<IActionResult> GetMovieLikedCount([FromRoute] Guid Id)
        {
            int result = await Mediator.Send(new GetMovieLikedCountQuery { MovieID = Id });

            return Ok(result);
        }

        [HttpGet("IsLiked/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> GetMovieLikedAny([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            bool result = await Mediator.Send(new GetMovieIsLikedQuery { MovieId = MovieId, UserId = UserId });

            return Ok(result);
        }

        [HttpDelete("UnlikeMovie/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> UnlikeMovie([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            await Mediator.Send(new LUnlikeMovieCommand { MovieId = MovieId, UserId = UserId });
            return Ok();
        }

        [HttpGet("MyLikedMovie")]
        public async Task<IActionResult> MyLikedMovie([FromQuery] int UserId, [FromQuery] PageRequest pageRequest)
        {
            GetMyLikedMovieQuery getListLikedMovieQuery = new() { UserId = UserId, pageRequest = pageRequest };

            GetListResponse<GetMyLikedMovieDto> result = await Mediator.Send(getListLikedMovieQuery);

            return Ok(result);
        }

        [HttpGet("GetLikedMovieSearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search, [FromQuery] int UserId)
        {
            GetListResponse<GetSearchLikedMovieDto> getListResponse = await Mediator.Send(new GetSearchLikedMovieQuery { pageRequest = pageRequest, Search = Search, UserId = UserId });
            return Ok(getListResponse);
        }
    }
}
