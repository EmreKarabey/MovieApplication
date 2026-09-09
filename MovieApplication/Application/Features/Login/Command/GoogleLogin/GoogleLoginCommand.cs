using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.Login;
using Application.Services.GoogleLoginService;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using CoreSecurity.JWT;
using Google.Apis.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Login.Command.GoogleLogin
{
    public class GoogleLoginCommand : IRequest<LoginResponse>, ICacheRemoveRequest, ILoggableRequest, ITransactionalRequest
    {
        public string Token { get; set; }

        public string? CacheKey => $"GoogleLoginCommand Token:{Token}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";

    }

    public class GoogleLoginHandler : IRequestHandler<GoogleLoginCommand, LoginResponse>
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserRepository _userRepository;
        private readonly IGoogleLoginService _googleLoginService;

        public GoogleLoginHandler(ITokenHelper tokenHelper, IUserRepository userRepository, IGoogleLoginService googleLoginService)
        {
            _tokenHelper = tokenHelper;
            _userRepository = userRepository;
            _googleLoginService = googleLoginService;
        }

        public async Task<LoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var payload = await _googleLoginService.GoogleLoginAsync(request.Token);

            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == payload.Email, include: n => n.Include(p => p.UserOperationClaims).ThenInclude(p => p.OperationClaim));

            if (user == null)
            {
                HashingKeyHelper.CreatePasswordHash(Guid.NewGuid().ToString(), out byte[] passwordHash, out byte[] passwordSalt);
                user = new User
                {
                    FirstName = payload.GivenName ?? "Google",
                    LastName = payload.FamilyName ?? "User",
                    EMail = payload.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true,
                    AccountType = "user",
                    TwoFactor = false,
                    DarkMode = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _userRepository.AddAsync(user);

            }

            List<OperationClaim> operationClaims = user.UserOperationClaims?.Select(n => n.OperationClaim).ToList() ?? new List<OperationClaim>();
            AccessToken access = _tokenHelper.CreateToken(user, operationClaims);
            return new LoginResponse { Token = access.Token, Expiration = access.Expiration, Requires2FA = false };
        }
    }
}