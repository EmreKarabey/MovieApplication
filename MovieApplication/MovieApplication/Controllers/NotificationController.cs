using System.Security.Claims;
using Application.Features.Forum.Command.Delete;
using Application.Features.LikedMovie.Queries.GetList;
using Application.Features.Notification.Command.Delete;
using Application.Features.Notification.Command.Read;
using Application.Features.Notification.Queries.GetMyNotificationList;
using CoreApplication.Request;
using CoreApplication.Responses;
using CoreSecurity.Entities;
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
    public class NotificationController : BaseController
    {
        [HttpGet("MyNotificationList")]
        public async Task<IActionResult> MyNotificationList([FromQuery] int UserId)
        {
            GetMyNotificationListQuery getMyNotificationListQuery = new() { UserId = UserId };

            List<GetMyNotificationListDto> result = await Mediator.Send(getMyNotificationListQuery);

            return Ok(result);
        }

        [HttpPost("SaveToken")]
        public async Task<IActionResult> SaveToken([FromBody] Application.Features.UserFCMToken.Commands.Create.CreateUserFCMTokenCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("Read")]
        public async Task<IActionResult> Read([FromBody] ReadNotificationCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{guid:guid}")]
        public async Task<IActionResult> RemoveNotification([FromRoute] Guid guid)
        {
            DeletedNotificationResponse deletedNotificationResponse = await Mediator.Send(new DeletedNotificationCommand() { Id = guid });
            return Ok(deletedNotificationResponse);
        }
    }
}
