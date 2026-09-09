using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Constants;
using Application.Features.Login.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using CoreSecurity.Entities;

namespace Application.Features.Login.Rules
{
    public class LoginBusinessRules : BaseBusinessRules
    {
        private readonly IUserRepository _userRepository;

        public LoginBusinessRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task NoUserFound(int Id)
        {
            User user = await _userRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (user == null) throw new BusinessException(LoginMessages.UserNotFound);
        }

    }
}
