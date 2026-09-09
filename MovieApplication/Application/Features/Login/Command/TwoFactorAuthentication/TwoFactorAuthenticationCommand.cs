using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.ResetPassword;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using CoreSecurity.OTPAuthenticator;
using MediatR;

namespace Application.Features.Login.Command.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommand : IRequest<string>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }
        public string? Code { get; set; }
        public string? SecretKey { get; set; }

        public string? CacheKey => $"TwoFactorAuthenticationCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class TwoFactorAuthenticationCommandHandler : IRequestHandler<TwoFactorAuthenticationCommand, string>
    {
        private readonly IOTPAuthenticator _oTPAuthenticator;
        private readonly IUserRepository _userRepository;
        private readonly IOTPAuthenticatorRepository _oTPAuthenticatorRepository;

        public TwoFactorAuthenticationCommandHandler(IOTPAuthenticator oTPAuthenticator, IUserRepository userRepository, IOTPAuthenticatorRepository oTPAuthenticatorRepository)
        {
            _oTPAuthenticator = oTPAuthenticator;
            _userRepository = userRepository;
            _oTPAuthenticatorRepository = oTPAuthenticatorRepository;
        }

        public async Task<string> Handle(TwoFactorAuthenticationCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user == null) throw new BusinessException("User Not Found");

            if (!user.TwoFactor)
            {
                if (string.IsNullOrEmpty(request.Code))
                {
                    byte[] key = await _oTPAuthenticator.GenerateSecretKey();
                    string stringKey = await _oTPAuthenticator.ConvertSecretKeyToString(key);
                    return stringKey;
                }
                else
                {
                    if (string.IsNullOrEmpty(request.SecretKey))
                        return "SecretKey is missing";

                    byte[] secretKeyBytes = await _oTPAuthenticator.ConvertStringToSecretKey(request.SecretKey);
                    bool isValid = await _oTPAuthenticator.VerfiyCode(secretKeyBytes, request.Code);

                    if (isValid)
                    {
                        var otp = new OTPAuthenticator(secretKey: secretKeyBytes, userId: user.EntityID, isVerifed: true);

                        await _oTPAuthenticatorRepository.AddAsync(otp);

                        user.TwoFactor = true;

                        await _userRepository.UpdateAsync(user);

                        return "Success";
                    }

                    return "Invalid Code";
                }
            }
            else
            {
                throw new BusinessException("Two-factor authentication is already enabled.");
            }

        }
    }
}
