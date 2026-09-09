using Application.Features.Announcement.Command.Create;
using Application.Features.Announcement.Command.Delete;
using Application.Features.Announcement.Command.Update;
using Application.Features.Announcement.Queries.GetById;
using Application.Features.Announcement.Queries.GetList;
using Application.Features.Announcement.Queries.GetLİst;
using Application.Features.Category.Command.Create;
using Application.Features.Category.Command.Delete;
using Application.Features.Category.Command.Update;
using Application.Features.Category.Queries.GetById;
using Application.Features.Category.Queries.GetList;
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
    public class AnnouncementController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            List<GetListAnnouncementDto> result = await Mediator.Send(new GetListAnnouncementQuery());

            return Ok(result);
        }


        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            GetByIdAnnouncementDto result = await Mediator.Send(new GetByIdAnnouncementQuery { Id = Id });

            return Ok(result);
        }


        [HttpPost("AddAnnouncement")]
        public async Task<IActionResult> AddAnnouncement([FromBody] CreatedAnnouncementCommand createdAnnouncementCommand)
        {
            CreatedAnnouncementResponse result = await Mediator.Send(createdAnnouncementCommand);

            return Ok(result);
        }

        [HttpPut("UpdateAnnouncement")]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] UpdateAnnouncementCommand updateAnnouncementCommand)
        {
            UpdateAnnouncementResponse result = await Mediator.Send(updateAnnouncementCommand);

            return Ok(result);
        }


        [HttpDelete("RemoveAnnouncement")]
        public async Task<IActionResult> RemoveAnnouncement([FromQuery] Guid Id)
        {
            DeletedAnnouncementResponse result = await Mediator.Send(new DeletedAnnouncementCommand { Id = Id });

            return Ok(result);
        }

    }
}
