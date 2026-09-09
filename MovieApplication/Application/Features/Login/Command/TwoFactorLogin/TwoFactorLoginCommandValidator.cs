using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Login.Command.TwoFactorLogin
{
    public class TwoFactorLoginCommandValidator : AbstractValidator<TwoFactorAuthenticationLoginCommand>
    {
        public TwoFactorLoginCommandValidator()
        {
            RuleFor(n => n.Email).NotEmpty().WithMessage("Email cannot be a blank");
            RuleFor(n => n.Code).NotEmpty().WithMessage("Code cannot be a blank");
        }
    }
}
