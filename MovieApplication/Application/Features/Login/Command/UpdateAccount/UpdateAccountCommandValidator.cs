using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Login.Command.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be a blank");

            RuleFor(n => n.FirstName).NotEmpty().WithMessage("FirstName cannot be a blank");
            RuleFor(n => n.LastName).NotEmpty().WithMessage("LastName cannot be a blank");


        }
    }
}
