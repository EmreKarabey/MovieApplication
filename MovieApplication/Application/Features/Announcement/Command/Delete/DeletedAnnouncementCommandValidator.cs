using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Announcement.Command.Delete
{
    public class DeletedAnnouncementCommandValidator : AbstractValidator<DeletedAnnouncementCommand>
    {
        public DeletedAnnouncementCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
