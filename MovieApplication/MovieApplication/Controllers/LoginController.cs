using System.Threading.Tasks;
using Application.Features.Login.Command.GoogleLogin;
using Application.Features.Login.Command.Login;
using Application.Features.Login.Command.ResetPassword;
using Application.Features.Login.Command.SenderCode;
using Application.Features.Login.Command.TwoFactorAuthentication;
using Application.Features.Login.Command.TwoFactorLogin;
using Application.Features.Login.Command.UpdateAccount;
using Application.Features.Login.Command.UpdateEmail;
using Application.Features.Register;
using CoreSecurity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MovieApplication.Controllers
{
    [EnableRateLimiting("AuthPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        [EnableRateLimiting("AuthPolicy")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        {
            LoginResponse loginResponse = await Mediator.Send(loginCommand);

            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand registerCommand)
        {
            RegisterResponse registerResponse = await Mediator.Send(registerCommand);

            return Ok(registerResponse);
        }

        [HttpPut("UpdateEmail")]
        public async Task<IActionResult> UpdateEmail(UpdateEmailCommand updateEmailCommand)
        {
            UpdateEmailResponse updateEmailResponse = await Mediator.Send(updateEmailCommand);

            return Ok(updateEmailResponse);
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode(SenderCodeCommand senderCodeCommand)
        {
            await Mediator.Send(senderCodeCommand);

            return Ok();
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            await Mediator.Send(resetPasswordCommand);

            return Ok();
        }

        [HttpPost("TwoFactorAuthentication")]
        public async Task<IActionResult> TwoFactorAuthentication(TwoFactorAuthenticationCommand twoFactorAuthenticationCommand)
        {
            var key = await Mediator.Send(twoFactorAuthenticationCommand);

            return Ok(key);
        }

        [HttpPost("TwoFactorAuthenticationLogin")]
        public async Task<IActionResult> TwoFactorAuthentication(TwoFactorAuthenticationLoginCommand twoFactorAuthenticationLoginCommand)
        {
            var key = await Mediator.Send(twoFactorAuthenticationLoginCommand);

            return Ok(key);
        }

        [HttpPost("DisableTwoFactorAuthentication")]
        public async Task<IActionResult> DisableTwoFactorAuthentication(Application.Features.Login.Command.DisableTwoFactorAuthentication.DisableTwoFactorAuthenticationCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("TwoFactorStatus/{email}")]
        public async Task<IActionResult> GetTwoFactorStatus(string email)
        {
            var query = new Application.Features.Login.Query.GetTwoFactorStatus.GetTwoFactorStatusQuery { Email = email };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(UpdateAccountCommand updateAccountCommand)
        {
            UpdateAccountResponse updateAccountResponse = await Mediator.Send(updateAccountCommand);

            return Ok(updateAccountResponse);
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand googleLoginCommand)
        {
            LoginResponse loginResponse = await Mediator.Send(googleLoginCommand);
            return Ok(loginResponse);
        }

    }
}
