using Application.Services.Repositories;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using MediatR;

namespace Application.Features.Login.Query.GetTwoFactorStatus
{
    public class GetTwoFactorStatusQuery : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class GetTwoFactorStatusQueryHandler : IRequestHandler<GetTwoFactorStatusQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public GetTwoFactorStatusQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(GetTwoFactorStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(n => n.EMail == request.Email);
            if (user == null) throw new BusinessException("User Not Found");

            return user.TwoFactor;
        }
    }
}
