using Application.Features.SubComments.Command.Create;
using Application.Features.SubComments.Command.Delete;
using Application.Features.SubComments.Command.Update;
using Application.Features.SubComments.Queries.GetById;
using Application.Features.SubComments.Queries.GetList;
using Application.Features.SubComments.Queries.GetCommentId;
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
    public class SubCommentsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListSubCommentQuery getListSubCommentQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListSubCommentDto> result = await Mediator.Send(getListSubCommentQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdSubCommentDto result = await Mediator.Send(new GetByIdSubCommentQuery { Id = Id });

            return Ok(result);
        }

        [HttpPost("AddSubComment")]
        public async Task<IActionResult> AddSubComment([FromBody] CreatedSubCommentCommand createdSubCommentCommand)
        {
            CreatedSubCommentResponse result = await Mediator.Send(createdSubCommentCommand);

            return Ok(result);
        }

        [HttpPut("UpdateSubComment")]
        public async Task<IActionResult> UpdateSubComment([FromBody] UpdateSubCommentCommand updateSubCommentCommand)
        {
            UpdateSubCommentResponse result = await Mediator.Send(updateSubCommentCommand);

            return Ok(result);
        }

        [HttpDelete("RemoveSubComment")]
        public async Task<IActionResult> RemoveSubComment([FromQuery] Guid Id)
        {
            DeletedSubCommentResponse result = await Mediator.Send(new DeletedSubCommentCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("GetByCommentId/{CommentId:guid}")]
        public async Task<IActionResult> GetByCommentId([FromRoute] Guid CommentId, [FromQuery] PageRequest pageRequest)
        {
            GetListResponse<GetByCommentSubCommentIdDto> result = await Mediator.Send(new GetByCommentSubCommentIdQuery { CommentID = CommentId, pageRequest = pageRequest });

            return Ok(result);
        }
    }
}



