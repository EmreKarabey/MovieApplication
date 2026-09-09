using Application.Features.Activities.Command.Create;
using Application.Features.Activities.Command.Delete;
using Application.Features.Activities.Command.Update;
using Application.Features.Activities.Queries.GetById;
using Application.Features.Activities.Queries.GetList;
using Application.Features.Activities.Queries.MyActivities;
using Application.Features.Activities.Queries.MyActivity;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Threading.Tasks;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitiesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListActivitiesQuery getListActivitiesQuery = new() { PageRequest = pageRequest };

            GetListResponse<GetListActivitiesDto> result = await Mediator.Send(getListActivitiesQuery);

            return Ok(result);
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdActivitiesDto result = await Mediator.Send(new GetByIdActivitiesQuery { Id = Id });

            return Ok(result);
        }

        [HttpPost("AddActivity")]
        public async Task<IActionResult> AddActivity([FromBody] CreatedActivitiesCommand createdActivitiesCommand)
        {
            CreatedActivitiesResponse result = await Mediator.Send(createdActivitiesCommand);

            return Ok(result);
        }

        [HttpPut("UpdateActivity")]
        public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivitiesCommand updateActivitiesCommand)
        {
            UpdateActivitiesResponse result = await Mediator.Send(updateActivitiesCommand);

            return Ok(result);
        }

        [HttpDelete("RemoveActivity")]
        public async Task<IActionResult> RemoveActivity([FromQuery] Guid Id)
        {
            DeletedActivitiesResponse result = await Mediator.Send(new DeletedActivitiesCommand { Id = Id });

            return Ok(result);
        }

        [HttpGet("MyActivities")]
        public async Task<IActionResult> MyActivities([FromQuery] int UserID, [FromQuery] PageRequest pageRequest)
        {
            GetListResponse<GetMyActivitiesDto> result = await Mediator.Send(new GetMyActivitiesQuery { UserID = UserID, PageRequest = pageRequest });

            return Ok(result);
        }
    }
}
