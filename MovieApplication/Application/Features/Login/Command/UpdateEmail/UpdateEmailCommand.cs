using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Command.Create;
using Application.Features.Announcement.Command.Update;
using Application.Features.Announcement.Rules;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.JWT;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Application.Features.Login.Command.UpdateEmail
{
    public class UpdateEmailCommand : IRequest<UpdateEmailResponse>, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public string Email { get; set; }
        public string NewEmail { get; set; }

        public int Code { get; set; }
        public string? CacheKey => $"UpdateEmailCommand Email:{NewEmail}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, UpdateEmailResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;


        public UpdateEmailCommandHandler(IUserRepository userRepository, IMapper mapper, ITokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<UpdateEmailResponse> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EMail == request.Email);

            if (user == null) throw new BusinessException("User Not Found or Incorrect code.");

            if (user.CodeDuration >= DateTime.UtcNow && user.Code == request.Code)
            {
                user.EMail = request.NewEmail;

                user.Code = null;
                user.CodeDuration = null;

                var entity = await _userRepository.UpdateAsync(user);

                var access = _tokenHelper.CreateToken(entity, new List<OperationClaim>());

                var result = _mapper.Map<UpdateEmailResponse>(entity);

                result.Expiration = access.Expiration;
                result.Token = access.Token;

                return result;
            }

            throw new BusinessException("Invalid or expired verification code.");
        }
    }
}
