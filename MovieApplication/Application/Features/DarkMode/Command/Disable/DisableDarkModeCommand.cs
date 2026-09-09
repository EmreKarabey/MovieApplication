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
using MediatR;

namespace Application.Features.DarkMode.Command.Disable
{
    public class DisableDarkModeCommand : IRequest, ITransactionalRequest, ICacheRemoveRequest, ILoggableRequest
    {
        public int UserId { get; set; }

        public string? CacheKey => $"DisableDarkModeCommand UserId:{UserId}";

        public bool ByPassCache { get; }

        public string? CacheGroupKey => "DarkModes";
    }

    public class DisableDarkModeCommandHandler : IRequestHandler<DisableDarkModeCommand>
    {
        private readonly IUserRepository _userRepository;

        public DisableDarkModeCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DisableDarkModeCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(predicate: n => n.EntityID == request.UserId);

            if (user == null) throw new BusinessException("User Not Found");

            user.DarkMode = false;

            await _userRepository.UpdateAsync(user);
        }
    }
}
