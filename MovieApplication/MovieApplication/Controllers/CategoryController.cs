using Application.Features.Category.Command.Create;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Queries.GetById;
using Application.Features.Category.Queries.GetList;
using Application.Features.Comment.Command.Delete;
using Application.Features.Comment.Command.Update;
using Application.Features.Movies.Commands.Create;
using Application.Features.Movies.Commands.Delete;
using Application.Features.Movies.Commands.Update;
using Application.Features.Movies.Queries.GetById;
using Application.Features.Movies.Queries.GetList;
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
    public class CategoryController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListCategoryQuery getListCategoryQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListCategoryDto> result = await Mediator.Send(getListCategoryQuery);

            return Ok(result);
        }


        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdCategoryDto result = await Mediator.Send(new GetByIdCategoryQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CreatedCategoryCommand createdCategoryCommand)
        {
            CreatedCategoryResponse result = await Mediator.Send(createdCategoryCommand);

            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdatedCategoryCommand updatedCategoryCommand)
        {
            UpdatedCategoryResponse result = await Mediator.Send(updatedCategoryCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveCategory")]
        public async Task<IActionResult> RemoveCategory([FromQuery] Guid Id)
        {
            DeletedCategoryResponse result = await Mediator.Send(new DeletedCategoryCommand { Id = Id });

            return Ok(result);
        }
    }
}
