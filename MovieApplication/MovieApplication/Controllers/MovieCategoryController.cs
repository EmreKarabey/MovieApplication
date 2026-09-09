using Application.Features.Movies.Commands.Create;
using Application.Features.Movies.Commands.Delete;
using Application.Features.Movies.Commands.Update;
using Application.Features.Movies.Queries.GetById;
using Application.Features.Movies.Queries.GetList;
using Application.Features.MoviesCategory.Commands.Create;
using Application.Features.MoviesCategory.Commands.Delete;
using Application.Features.MoviesCategory.Commands.Update;
using Application.Features.MoviesCategory.Queries.GetById;
using Application.Features.MoviesCategory.Queries.GetCategoryName;
using Application.Features.MoviesCategory.Queries.GetList;
using Application.Features.MoviesCategory.Queries.GetMovieId;
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
    public class MovieCategoryController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListMovieCategoryQuery getListMovieCategoryQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetListMovieCategoryDto> result = await Mediator.Send(getListMovieCategoryQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdMovieCategoryDto result = await Mediator.Send(new GetByIdMovieCategoryQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddMovieCategory")]
        public async Task<IActionResult> AddMovieCategory([FromBody] CreatedMoviesCategoryCommand createdMoviesCategoryCommand)
        {
            CreatedMoviesCategoryResponse result = await Mediator.Send(createdMoviesCategoryCommand);

            return Ok(result);
        }

        [HttpPut("UpdateMovieCategory")]
        public async Task<IActionResult> UpdateMovieCategory([FromBody] UpdatedMovieCategoryCommand updatedMovieCategoryCommand)
        {
            UpdatedMovieCategoryResponse result = await Mediator.Send(updatedMovieCategoryCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveMovieCategory")]
        public async Task<IActionResult> RemoveMovieCategory([FromQuery] Guid Id)
        {
            DeletedMovieCategoryResponse result = await Mediator.Send(new DeletedMovieCategoryCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("GetMovie/{MovieId:guid}")]
        public async Task<IActionResult> GetMovie([FromRoute] Guid MovieId)
        {
            GetByMovieDto result = await Mediator.Send(new GetByMovieQuery { MovieID = MovieId });

            return Ok(result);
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory([FromQuery] PageRequest pageRequest, [FromQuery] string? CategoryName = null)
        {
            List<GetCategoryNameDto> result = await Mediator.Send(new GetCategoryNameQuery { pageRequest = pageRequest, CategoryName = CategoryName });

            return Ok(result);
        }
    }
}
