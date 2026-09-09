using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Notification.Command.Delete
{
    public class DeletedNotificationCommandValidator : AbstractValidator<DeletedNotificationCommand>
    {
        public DeletedNotificationCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }

    }
}
