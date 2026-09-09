using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Login.Command.UpdateEmail
{
    public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
    {
        public UpdateEmailCommandValidator()
        {
            RuleFor(n => n.NewEmail).NotEmpty().WithMessage("Email cannot be blank");
            RuleFor(n => n.Code).NotEmpty().WithMessage("Code cannot be blank");
        }
    }
}
