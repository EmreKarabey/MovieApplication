using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Login.Command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(n => n.Email).NotEmpty().WithMessage("Email cannot be a blank").EmailAddress().WithMessage("Email cannot be left blank.");
            RuleFor(n => n.Password).NotEmpty().WithMessage("Password cannot be left blank.");
        }
    }
}
