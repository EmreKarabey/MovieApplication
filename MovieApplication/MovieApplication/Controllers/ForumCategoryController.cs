using Application.Features.Forum.Command.Create;
using Application.Features.Forum.Command.Delete;
using Application.Features.Forum.Command.Update;
using Application.Features.Forum.Queries.GetById;
using Application.Features.Forum.Queries.GetList;
using Application.Features.ForumCategory.Command.Create;
using Application.Features.ForumCategory.Command.Delete;
using Application.Features.ForumCategory.Command.Update;
using Application.Features.ForumCategory.Queries.GetById;
using Application.Features.ForumCategory.Queries.GetList;
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
    public class ForumCategoryController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListForumCategoryQuery getListForumCategoryQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListForumCategoryDto> getListResponse = await Mediator.Send(getListForumCategoryQuery);
            return Ok(getListResponse);
        }

        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid guid)
        {
            GetByIdForumCategoryQuery getByIdForumCategoryQuery = new() { Id = guid };

            GetByIdForumCategoryDto getByIdForumCategoryDto = await Mediator.Send(getByIdForumCategoryQuery);
            return Ok(getByIdForumCategoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddForumCategory([FromBody] CreatedForumCategoryCommand createdForumCategoryCommand)
        {
            CreatedForumCategoryResponse createdForumCategoryResponse = await Mediator.Send(createdForumCategoryCommand);
            return Ok(createdForumCategoryResponse);
        }

        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> RemoveForumCategory([FromRoute] Guid guid)
        {
            DeletedForumCategoryResponse deletedForumCategoryResponse = await Mediator.Send(new DeletedForumCategoryCommand() { Id = guid });
            return Ok(deletedForumCategoryResponse);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateForumCategory([FromBody] UpdatedForumCategoryCommand updatedForumCategoryCommand)
        {
            UpdatedForumCategoryResponse updatedForumCategoryResponse = await Mediator.Send(updatedForumCategoryCommand);
            return Ok(updatedForumCategoryResponse);
        }
    }
}
