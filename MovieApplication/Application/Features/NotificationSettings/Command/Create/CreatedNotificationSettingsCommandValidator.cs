using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.NotificationSettings.Command.Create
{
    public class CreatedNotificationSettingsCommandValidator : AbstractValidator<CreatedNotificationSettingsCommand>
    {
        public CreatedNotificationSettingsCommandValidator()
        {
            RuleFor(n => n.IsNotificationEnabled).NotNull();
            RuleFor(n => n.ChannelId).NotNull();
        }
    }
}
