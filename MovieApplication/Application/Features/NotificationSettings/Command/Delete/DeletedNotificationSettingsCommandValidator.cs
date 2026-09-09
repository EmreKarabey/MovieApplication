using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.NotificationSettings.Command.Delete
{
    public class DeletedNotificationSettingsCommandValidator : AbstractValidator<DeletedNotificationSettingsCommand>
    {
        public DeletedNotificationSettingsCommandValidator()
        {
            RuleFor(n => n.EntityID).NotEmpty().WithMessage("EntityID cannot be blank");
        }
    }
}
