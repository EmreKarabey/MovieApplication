using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.Login;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.JWT;
using CoreSecurity.OTPAuthenticator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Login.Command.TwoFactorLogin
{
    public class TwoFactorAuthenticationLoginCommand : IRequest<LoginResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }

        public string? CacheKey => $"TwoFactorAuthenticationLoginCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }
    public class TwoFactorAuthenticationLoginCommandHandler : IRequestHandler<TwoFactorAuthenticationLoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOTPAuthenticator _oTPAuthenticator;
        private readonly IOTPAuthenticatorRepository _oTPAuthenticatorRepository;
        private readonly ITokenHelper _tokenHelper;

        public TwoFactorAuthenticationLoginCommandHandler(IUserRepository userRepository, IOTPAuthenticator oTPAuthenticator, IOTPAuthenticatorRepository oTPAuthenticatorRepository, ITokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _oTPAuthenticator = oTPAuthenticator;
            _oTPAuthenticatorRepository = oTPAuthenticatorRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<LoginResponse> Handle(TwoFactorAuthenticationLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
                predicate: n => n.EMail == request.Email,
                include: u => u.Include(x => x.UserOperationClaims).ThenInclude(x => x.OperationClaim)
            );
            if (user == null) throw new BusinessException("User Not Found");

            if (!user.TwoFactor) throw new BusinessException("Two-factor authentication is not enabled.");

            var opt = await _oTPAuthenticatorRepository.GetAsync(predicate: n => n.UserId == user.EntityID);

            if (opt == null) throw new BusinessException("OtpAuthenticator Not Found");

            byte[] savedSecretKey = opt.SecretKey;

            bool isCodeCorrect = await _oTPAuthenticator.VerfiyCode(savedSecretKey, request.Code);

            if (!isCodeCorrect) throw new Exception("Password is incorrect");

            List<OperationClaim> operationClaims = user.UserOperationClaims
                .Select(uoc => uoc.OperationClaim)
                .ToList();

            AccessToken access = _tokenHelper.CreateToken(user, operationClaims);

            return new LoginResponse { Token = access.Token, Expiration = access.Expiration };
        }
    }
}
