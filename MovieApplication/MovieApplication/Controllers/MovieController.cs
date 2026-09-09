using System.Threading.Tasks;
using Application.Features.Movies.Commands.Create;
using Application.Features.Movies.Commands.Delete;
using Application.Features.Movies.Commands.Update;
using Application.Features.Movies.Queries.GetById;
using Application.Features.Movies.Queries.GetList;
using Application.Features.Movies.Queries.SearchMovie;
using Application.Features.UnlikedMovie.Queries.SearchUnlikedMovie;
using CoreApplication.Request;
using CoreApplication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetMovieListQuery getMovieListQuery = new() { pageRequest = pageRequest };

            GetListResponse<GetMoviesListDto> result = await Mediator.Send(getMovieListQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdMovieDto result = await Mediator.Send(new GetByIdMovieQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovie([FromForm] CreatedMovieCommand createdMovieCommand)
        {
            CreatedMovieResponse result = await Mediator.Send(createdMovieCommand);

            return Ok(result);
        }

        [HttpPut("UpdateMovie")]
        public async Task<IActionResult> UpdateMovie([FromForm] UpdatedMovieCommand updatedMovieCommand)
        {
            UpdatedMovieResponse result = await Mediator.Send(updatedMovieCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveMovie")]
        public async Task<IActionResult> RemoveMovie([FromBody] DeletedMovieCommand deletedMovieCommand)
        {
            DeletedMovieResponse result = await Mediator.Send(deletedMovieCommand);

            return Ok(result);
        }

        [HttpGet("GetMovieSearch")]
        public async Task<IActionResult> Search([FromQuery] PageRequest pageRequest, [FromQuery] string? Search)
        {
            GetListResponse<GetSearchMovieDto> getListResponse = await Mediator.Send(new GetSearchMovieQuery { pageRequest = pageRequest, Search = Search });
            return Ok(getListResponse);
        }
    }
}
