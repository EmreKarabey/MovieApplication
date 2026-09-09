using Application.Services.Repositories;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserFCMToken.Commands.Create
{
    public class CreateUserFCMTokenCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }

    public class CreateUserFCMTokenCommandHandler : IRequestHandler<CreateUserFCMTokenCommand, bool>
    {
        private readonly IUserFCMTokenRepository _userFCMTokenRepository;

        public CreateUserFCMTokenCommandHandler(IUserFCMTokenRepository userFCMTokenRepository)
        {
            _userFCMTokenRepository = userFCMTokenRepository;
        }

        public async Task<bool> Handle(CreateUserFCMTokenCommand request, CancellationToken cancellationToken)
        {
            var existingToken = await _userFCMTokenRepository.GetAsync(t => t.UserId == request.UserId && t.Token == request.Token);
            if (existingToken != null) return true;

            var userFCMToken = new Domain.Entities.UserFCMToken
            {
                UserId = request.UserId,
                Token = request.Token
            };

            await _userFCMTokenRepository.AddAsync(userFCMToken);
            return true;
        }
    }
}
