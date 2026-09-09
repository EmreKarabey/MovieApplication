using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Create;
using Application.Services.Repositories;
using CoreApplication.Pipelines.Caching;
using CoreApplication.Pipelines.Logging;
using CoreApplication.Pipelines.Transaction;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;
using MediatR;

namespace Application.Features.DarkMode.Command
{
    public class ActiveDarkModeCommand : IRequest, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public int UserId { get; set; }

        public string? CacheKey => $"ActiveDarkModeCommand UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "DarkModes";
    }

    public class ActiveDarkModeCommandHandler : IRequestHandler<ActiveDarkModeCommand>
    {
        private readonly IUserRepository _userRepository;

        public ActiveDarkModeCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(ActiveDarkModeCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EntityID == request.UserId);

            if (user == null) throw new BusinessException("User Not Found");

            user.DarkMode = true;

            await _userRepository.UpdateAsync(user);
        }
    }
}
