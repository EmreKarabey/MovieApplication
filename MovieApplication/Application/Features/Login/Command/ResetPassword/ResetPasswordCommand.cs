using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.UpdateEmail;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.Hashing;
using CoreSecurity.JWT;
using MediatR;

namespace Application.Features.Login.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }
        public int Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string? CacheKey => $"ResetPasswordCommand Email:{Email}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;


        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user == null) throw new BusinessException("User Not Found or Incorrect code.");

            if (user.CodeDuration >= DateTime.UtcNow && user.Code == request.Code)
            {
                HashingKeyHelper.CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                user.Code = null;
                user.CodeDuration = null;

                await _userRepository.UpdateAsync(user);


            }
            else
            {
                throw new BusinessException("Invalid or expired verification code.");
            }


        }
    }
}
