using Application.Features.Comment.Command.Update;
using Application.Features.DarkMode.Command;
using Application.Features.DarkMode.Command.Disable;
using Application.Features.DarkMode.Queries.IsDarkMode;
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
    public class DarkModeController : BaseController
    {
        [HttpPut("ActiveDarkMode")]
        public async Task<IActionResult> ActiveDarkMode([FromBody] ActiveDarkModeCommand activeDarkModeCommand)
        {
            await Mediator.Send(activeDarkModeCommand);

            return Ok();
        }

        [HttpPut("DisableDarkMode")]
        public async Task<IActionResult> DisableDarkMode([FromBody] DisableDarkModeCommand disableDarkModeCommand)
        {
            await Mediator.Send(disableDarkModeCommand);

            return Ok();
        }

        [HttpGet("IsDarkMode/{Id:int}")]
        public async Task<IActionResult> IsDarkMode([FromRoute] int Id)
        {
            var result = await Mediator.Send(new GetIsDarkModeQuery { UserId = Id });

            return Ok(result);
        }

    }
}
