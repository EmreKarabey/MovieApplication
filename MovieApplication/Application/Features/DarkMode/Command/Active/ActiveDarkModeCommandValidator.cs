using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.DarkMode.Command.Active
{
    public class ActiveDarkModeCommandValidator : AbstractValidator<ActiveDarkModeCommand>
    {
        public ActiveDarkModeCommandValidator()
        {
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be blank");
        }
    }
}
