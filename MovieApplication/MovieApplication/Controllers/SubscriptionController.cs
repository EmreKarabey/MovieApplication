using System.Threading.Tasks;
using Application.Features.Subscription.Command.Unsubscribe;
using Application.Features.Subscription.Queries.IsSubscribe;
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
    public class SubscriptionController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Application.Features.Subscription.Command.Create.CreatedSubscriptionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Application.Features.Subscription.Command.Update.UpdatedSubscriptionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new Application.Features.Subscription.Command.Delete.DeletedSubscriptionCommand { EntityID = id });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await Mediator.Send(new Application.Features.Subscription.Queries.GetById.GetByIdSubscriptionQuery { EntityID = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CoreApplication.Request.PageRequest pageRequest)
        {
            var result = await Mediator.Send(new Application.Features.Subscription.Queries.GetList.GetListSubscriptionQuery { pageRequest = pageRequest });
            return Ok(result);
        }

        [HttpGet("IsSubscribe/{ChannelId:int}")]
        public async Task<IActionResult> IsSubscribe(int ChannelId)
        {
            var result = await Mediator.Send(new GetIsSubscribeQuery { ChannelId = ChannelId });
            return Ok(result);
        }

        [HttpDelete("Unsubscribe/{ChannelId:int}")]
        public async Task<IActionResult> Unsubscribe(int ChannelId)
        {
            await Mediator.Send(new UnsubscribeCommand { ChannelId = ChannelId });
            return Ok();
        }
    }
}
