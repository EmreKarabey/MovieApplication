using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Notification.Command.Create
{
    public class CreatedNotificationCommandValidator : AbstractValidator<CreatedNotificationCommand>
    {
        public CreatedNotificationCommandValidator()
        {
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be blank");
            RuleFor(n => n.Message).NotEmpty().WithMessage("Message cannot be blank");
            RuleFor(n => n.Url).NotEmpty().WithMessage("Url cannot be blank");
        }
    }
}
