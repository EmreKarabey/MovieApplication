using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Login.Command.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(n => n.Email).NotEmpty().WithMessage("Email cannot be blank");
            RuleFor(n => n.NewPassword).NotEmpty().WithMessage("NewPassword cannot be blank");
            RuleFor(n => n.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword cannot be blank");
            RuleFor(n => n.Code).NotEmpty().WithMessage("Code cannot be blank");

            RuleFor(n => n.NewPassword).Equal(n => n.ConfirmPassword).WithMessage("Passwords must match.");
        }
    }
}
