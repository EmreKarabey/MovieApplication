using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.DarkMode.Command.Disable
{
    public class DisableDarkModeCommandHandlerValidator : AbstractValidator<ActiveDarkModeCommand>
    {
        public DisableDarkModeCommandHandlerValidator()
        {
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be blank");
        }
    }
}
