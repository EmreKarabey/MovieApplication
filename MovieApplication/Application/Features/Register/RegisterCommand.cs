using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using CoreSecurity.JWT;
using MediatR;

namespace Application.Features.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>, ILoggableRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? CacheKey => $"RegisterCommand";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Register";
    }

    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        public RegisterHandler(IUserRepository userRepository, ITokenHelper tokenHelper, IUserOperationClaimRepository userOperationClaimRepository)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _userOperationClaimRepository = userOperationClaimRepository;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user != null) throw new BusinessException("This email is already registered.");

            HashingKeyHelper.CreatePasswordHash(Password: request.Password,
               PasswordHash: out byte[] passwordHash,
               PasswordSalt: out byte[] passwordSalt);

            User newUser = new User
            {
                FirstName = request.FirstName,
                CreatedAt = DateTime.UtcNow,
                LastName = request.LastName,
                Status = true,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                EMail = request.Email
            };

            var addUser = await _userRepository.AddAsync(newUser);

            UserOperationClaim userOperationClaim = new UserOperationClaim
            {
                OperationClaimID = 4,
                UserID = addUser.EntityID
            };

            await _userOperationClaimRepository.AddAsync(userOperationClaim);

            AccessToken accessToken = _tokenHelper.CreateToken(newUser, new List<OperationClaim>());

            return new RegisterResponse
            {
                Expiration = accessToken.Expiration,
                Token = accessToken.Token
            };
        }
    }
}
