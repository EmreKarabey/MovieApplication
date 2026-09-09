using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.NotificationSettings.Command.Update
{
    public class UpdatedNotificationSettingsCommandValidator : AbstractValidator<UpdatedNotificationSettingsCommand>
    {
        public UpdatedNotificationSettingsCommandValidator()
        {
            RuleFor(n => n.EntityID).NotEmpty().WithMessage("EntityID cannot be blank");
        }
    }
}
