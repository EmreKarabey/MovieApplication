using Application.Features.Category.Command.Create;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Queries.GetById;
using Application.Features.Category.Queries.GetList;
using Application.Features.Comment.Command.Create;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Command.Update;
using Application.Features.Comment.Queries.GetById;
using Application.Features.Comment.Queries.GetList;
using Application.Features.Comment.Queries.GetMovieId;
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
    public class CommentController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListCommentQuery getListCommentQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListCommentDto> result = await Mediator.Send(getListCommentQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdCommentDto result = await Mediator.Send(new GetByIdCommentQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CreatedCommentCommand createdCommentCommand)
        {
            CreatedCommentResponse result = await Mediator.Send(createdCommentCommand);

            return Ok(result);
        }

        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentCommand updateCommentCommand)
        {
            UpdateCommentResponse result = await Mediator.Send(updateCommentCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveComment")]
        public async Task<IActionResult> RemoveComment([FromQuery] Guid Id)
        {
            DeletedCommentResponse result = await Mediator.Send(new DeletedCommentCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("GetByMovieId/{MovieId:guid}")]
        public async Task<IActionResult> GetByMovieId([FromRoute] Guid MovieId, [FromQuery] PageRequest pageRequest)
        {
            GetListResponse<GetByMovieCommentIdDto> result = await Mediator.Send(new GetByMovieCommentIdQuery { MovieID = MovieId, pageRequest = pageRequest });

            return Ok(result);
        }
    }
}
