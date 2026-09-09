using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using MediatR;

namespace Application.Features.Login.Command.DisableTwoFactorAuthentication
{
    public class DisableTwoFactorAuthenticationCommand : IRequest<string>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }

        public string? CacheKey => $"TwoFactorAuthenticationCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class DisableTwoFactorAuthenticationCommandHandler : IRequestHandler<DisableTwoFactorAuthenticationCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOTPAuthenticatorRepository _oTPAuthenticatorRepository;

        public DisableTwoFactorAuthenticationCommandHandler(IUserRepository userRepository, IOTPAuthenticatorRepository oTPAuthenticatorRepository)
        {
            _userRepository = userRepository;
            _oTPAuthenticatorRepository = oTPAuthenticatorRepository;
        }

        public async Task<string> Handle(DisableTwoFactorAuthenticationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user == null) throw new BusinessException("User Not Found");

            if (user.TwoFactor)
            {
                user.TwoFactor = false;
                await _userRepository.UpdateAsync(user);

                // optionally delete the existing authenticator record if it exists
                var existingOtp = await _oTPAuthenticatorRepository.GetAsync(predicate: o => o.UserId == user.EntityID);
                if (existingOtp != null)
                {
                    await _oTPAuthenticatorRepository.DeleteAsync(existingOtp);
                }

                return "Success";
            }
            else
            {
                throw new BusinessException("Two-factor authentication is already disabled.");
            }
        }
    }
}
