using System.Threading.Tasks;
using Application.Features.Forum.Command.Create;
using Application.Features.Forum.Command.Delete;
using Application.Features.Forum.Command.Update;
using Application.Features.Forum.Queries.ActiveForumList;
using Application.Features.Forum.Queries.GetById;
using Application.Features.Forum.Queries.GetList;
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
    public class ForumController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListForumQuery getListForumQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListForumDto> getListResponse = await Mediator.Send(getListForumQuery);
            return Ok(getListResponse);
        }

        [HttpGet("ActiveList")]
        public async Task<IActionResult> ActiveList([FromQuery] PageRequest pageRequest)
        {
            GetActiveForumListQuery getActiveForumListQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetActiveForumListDto> getListResponse = await Mediator.Send(getActiveForumListQuery);
            return Ok(getListResponse);
        }

        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid guid)
        {
            GetByIdForumQuery getByIdForumQuery = new() { Id = guid };

            GetByIdForumDto getByIdForumDto = await Mediator.Send(getByIdForumQuery);
            return Ok(getByIdForumDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum([FromBody] CreatedForumCommand createdForumCommand)
        {
            CreatedForumResponse createdForumResponse = await Mediator.Send(createdForumCommand);
            return Ok(createdForumResponse);
        }

        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> RemoveForum([FromRoute] Guid guid)
        {
            DeletedForumResponse deletedForumResponse = await Mediator.Send(new DeletedForumCommand() { Id = guid });
            return Ok(deletedForumResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateForum([FromBody] UpdateForumCommand updateForumCommand)
        {
            UpdateForumResponse updateForumResponse = await Mediator.Send(updateForumCommand);
            return Ok(updateForumResponse);
        }
    }
}
