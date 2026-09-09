using System;
using System.Threading.Tasks;
using Application.Features.NotificationSettings.Command.ChangeSettings;
using Application.Features.NotificationSettings.Command.Create;
using Application.Features.NotificationSettings.Command.Delete;
using Application.Features.NotificationSettings.Command.Update;
using Application.Features.NotificationSettings.Queries.GetById;
using Application.Features.NotificationSettings.Queries.GetList;
using CoreApplication.Request;
using CoreApplication.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("GeneralPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationSettingsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListNotificationSettingsQuery query = new() { pageRequest = pageRequest };
            GetListResponse<GetListNotificationSettingsDto> result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetByIdNotificationSettingsDto result = await Mediator.Send(new GetByIdNotificationSettingsQuery { EntityID = id });
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreatedNotificationSettingsCommand command)
        {
            CreatedNotificationSettingsResponse result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdatedNotificationSettingsCommand command)
        {
            UpdatedNotificationSettingsResponse result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            DeletedNotificationSettingsResponse result = await Mediator.Send(new DeletedNotificationSettingsCommand { EntityID = id });
            return Ok(result);
        }

        [HttpPut("ChangeNotificationSetting")]
        public async Task<IActionResult> ChangeNotificationSetting([FromBody] ChangeNotificationSettingsCommand command)
        {
            ChangeNotificationSettingsResponse result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
