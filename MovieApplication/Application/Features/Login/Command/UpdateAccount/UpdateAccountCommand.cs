using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Login.Command.SenderCode;
using Application.Services;
using Application.Services.Repositories;
using AutoMapper;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using CoreSecurity.JWT;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Login.Command.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<UpdateAccountResponse>, ILoggableRequest, ICacheRemoveRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? CacheKey => $"UpdateAccountCommand Id:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "Login";
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHelper _tokenHelper;

        public UpdateAccountCommandHandler(IUserRepository userRepository, ITokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<UpdateAccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EntityID == request.UserId);

            if (user == null) throw new BusinessException("User Not Found");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            AccessToken access = _tokenHelper.CreateToken(user, new List<OperationClaim>());

            var result = new UpdateAccountResponse
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Token = access.Token
            };

            return result;

        }
    }
}
