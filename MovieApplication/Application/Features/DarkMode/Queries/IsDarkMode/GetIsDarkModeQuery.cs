using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetList;
using Application.Features.DarkMode.Command;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Request;
using CoreApplication.Responses;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using MediatR;

namespace Application.Features.DarkMode.Queries.IsDarkMode
{
    public class GetIsDarkModeQuery : IRequest<bool>, ILoggableRequest, ICachableRequest
    {
        public int UserId { get; set; }
        public string? CacheKey => $"GetIsDarkModeQuery UserId:{UserId}";

        public bool ByPassCache => true;

        public string? CacheGroupKey => "DarkModes";

        public TimeSpan? SlidingExpiration { get; }
    }

    public class GetIsDarkModeQueryHandler : IRequestHandler<GetIsDarkModeQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public GetIsDarkModeQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(GetIsDarkModeQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EntityID == request.UserId);

            if (user == null) throw new BusinessException("User Not Found");

            if (user.DarkMode) return true;

            return false;
        }
    }
}