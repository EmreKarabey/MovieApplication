using Application.Features.FavoriteMovie.Queries.MyFavoriteMovie;
using Application.Features.History.Command.Create;
using Application.Features.History.Command.Delete;
using Application.Features.History.Command.Update;
using Application.Features.History.Queries.GetById;
using Application.Features.History.Queries.GetList;
using Application.Features.History.Queries.MyHistory;
using Application.Features.History.Queries.MyStatics;
using Application.Features.History.Queries.Search;
using Application.Features.LikedMovie.Command.Create;
using Application.Features.LikedMovie.Command.Delete;
using Application.Features.LikedMovie.Command.UnlikeMovie;
using Application.Features.LikedMovie.Command.Update;
using Application.Features.LikedMovie.Queries.GetById;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.LikedMovie.Queries.GetMovieLikedCount;
using Application.Features.LikedMovie.Queries.IsLiked;
using Application.Features.SavedMovie.Queries.SearchSaveMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListHistoryQuery getListHistoryQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListHistoryDto> result = await Mediator.Send(getListHistoryQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdHistoryDto result = await Mediator.Send(new GetByIdHistoryQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddHistory")]
        public async Task<IActionResult> AddHistory([FromBody] CreatedHistoryCommand createdHistoryCommand)
        {
            CreatedHistoryResponse result = await Mediator.Send(createdHistoryCommand);

            return Ok(result);
        }

        [HttpPut("UpdateHistory")]
        public async Task<IActionResult> UpdateHistory([FromBody] UpdateHistoryCommand updateHistoryCommand)
        {
            UpdateHistoryResponse result = await Mediator.Send(updateHistoryCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveHistory")]
        public async Task<IActionResult> RemoveHistory([FromQuery] Guid Id)
        {
            DeletedHistoryResponse result = await Mediator.Send(new DeletedHistoryCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("MyHistory/{userId:int}")]
        public async Task<IActionResult> GetMyHistoryMovie([FromRoute] int userId, [FromQuery] PageRequest pageRequest)
        {
            GetMyHistoryQuery getMyHistoryQuery = new() { pageRequest = pageRequest, UserId = userId };

            GetListResponse<GetMyHistoryDto> result = await Mediator.Send(getMyHistoryQuery);

            return Ok(result);

        }

        [HttpGet("GetHistorySearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search, [FromQuery] int UserId)
        {
            GetListResponse<GetSearchHistoryDto> getListResponse = await Mediator.Send(new GetSearchHistoryQuery { pageRequest = pageRequest, Search = Search, UserId = UserId });
            return Ok(getListResponse);
        }

        [HttpGet("MyStatics")]
        public async Task<IActionResult> MyStatics([FromQuery] int UserId)
        {
            GetMyStaticsMovieDto getMyStaticsMovieDto = await Mediator.Send(new GetMyStaticsMovieQuery { UserId = UserId });
            return Ok(getMyStaticsMovieDto);
        }

    }
}
