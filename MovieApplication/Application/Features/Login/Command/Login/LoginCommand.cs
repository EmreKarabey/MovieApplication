using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Authorization;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using CoreSecurity.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Login.Command.Login
{
    public class LoginCommand : IRequest<LoginResponse>, ILoggableRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string? CacheKey => $"LoginCommand Email:{Email}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserRepository _userRepository;

        public LoginHandler(ITokenHelper tokenHelper, IUserRepository userRepository)
        {
            _tokenHelper = tokenHelper;
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email,
                include: n => n.Include(n => n.UserOperationClaims).ThenInclude(n => n.OperationClaim));

            if (user == null) throw new BusinessException("User Not Found");

            bool isValid = HashingKeyHelper.VerifyPasswordHash(Password: request.Password, PasswordHash: user.PasswordHash, PasswordSalt: user.PasswordSalt);

            if (!isValid) throw new Exception("Password is incorrect");

            if (user.TwoFactor)
            {
                return new LoginResponse { Requires2FA = true };
            }

            List<OperationClaim> operationClaims = user.UserOperationClaims
                .Select(n => n.OperationClaim).ToList();

            AccessToken access = _tokenHelper.CreateToken(user, operationClaims);

            return new LoginResponse { Token = access.Token, Expiration = access.Expiration, Requires2FA = false };
        }
    }
}
