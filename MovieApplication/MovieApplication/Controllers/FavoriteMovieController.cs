using Application.Features.FavoriteMovie.Command.Create;
using Application.Features.FavoriteMovie.Command.Delete;
using Application.Features.FavoriteMovie.Command.RemoveByMovieId;
using Application.Features.FavoriteMovie.Command.Update;
using Application.Features.FavoriteMovie.Queries.GetById;
using Application.Features.FavoriteMovie.Queries.GetCategoryName;
using Application.Features.FavoriteMovie.Queries.GetList;
using Application.Features.FavoriteMovie.Queries.GetMovieFavoritedCount;
using Application.Features.FavoriteMovie.Queries.IsFavorited;
using Application.Features.FavoriteMovie.Queries.MyFavoriteMovie;
using Application.Features.FavoriteMovie.Queries.SearchFavoriteMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteMovieController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListFavoriteMovieQuery getListFavoriteMovieQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListFavoriteMovieDto> result = await Mediator.Send(getListFavoriteMovieQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdFavoriteMovieDto result = await Mediator.Send(new GetByIdFavoriteMovieQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddFavoriteMovie")]
        public async Task<IActionResult> AddFavoriteMovie([FromBody] CreatedFavoriteMovieCommand createdFavoriteMovieCommand)
        {
            CreatedFavoriteMovieResponse result = await Mediator.Send(createdFavoriteMovieCommand);

            return Ok(result);
        }

        [HttpPut("UpdateFavoriteMovie")]
        public async Task<IActionResult> UpdateFavoriteMovie([FromBody] UpdateFavoriteMovieCommand updateFavoriteMovieCommand)
        {
            UpdateFavoriteMovieResponse result = await Mediator.Send(updateFavoriteMovieCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveFavoriteMovie")]
        public async Task<IActionResult> RemoveFavoriteMovie([FromQuery] Guid Id)
        {
            DeletedFavoriteMovieResponse result = await Mediator.Send(new DeletedFavoriteMovieCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("MovieFavoritedCount/{Id:guid}")]
        public async Task<IActionResult> GetMovieFavoritedCount([FromRoute] Guid Id)
        {
            int result = await Mediator.Send(new GetMovieFavoritedCountQuery { MovieID = Id });

            return Ok(result);
        }

        [HttpGet("IsFavorited/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> GetMovieLikedAny([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            bool result = await Mediator.Send(new GetMovieIsFavoritedQuery { MovieId = MovieId, UserId = UserId });

            return Ok(result);
        }

        [HttpDelete("RemoveByMovieId/{MovieId:guid}/{UserId:int}")]
        public async Task<IActionResult> RemoveByMovieId([FromRoute] Guid MovieId, [FromRoute] int UserId)
        {
            await Mediator.Send(new RemoveByMovieIdCommand { MovieId = MovieId, UserId = UserId });
            return Ok();
        }

        [HttpGet("MyFavorite/{userId:int}")]
        public async Task<IActionResult> GetMyFavoriteMovie([FromRoute] int userId, [FromQuery] PageRequest pageRequest)
        {
            GetMyFavoriteMovieQuery getMyFavoriteMovieQuery = new() { pageRequest = pageRequest, UserId = userId };

            GetListResponse<GetMyFavoriteMovieDto> result = await Mediator.Send(getMyFavoriteMovieQuery);

            return Ok(result);

        }

        [HttpGet("GetFavoriteCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] PageRequest pageRequest, [FromQuery] int UserId, [FromQuery] string? CategoryName = null)
        {
            List<GetFavoriteCategoryNameDto> result = await Mediator.Send(new GetFavoriteCategoryNameQuery { pageRequest = pageRequest, CategoryName = CategoryName, UserId = UserId });

            return Ok(result);
        }

        [HttpGet("GetFavoriteMovieSearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search, [FromQuery] int UserId)
        {
            GetListResponse<GetSearchFavoriteMovieDto> getSearchFavoriteMovieDtos = await Mediator.Send(new GetSearchFavoriteMovieQuery { pageRequest = pageRequest, Search = Search, UserId = UserId });
            return Ok(getSearchFavoriteMovieDtos);
        }
    }
}

