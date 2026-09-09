using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Announcement.Command.Update
{
    public class UpdateAnnouncementCommandValidator : AbstractValidator<UpdateAnnouncementCommand>
    {
        public UpdateAnnouncementCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.Title).NotEmpty().WithMessage("Title cannot be blank");
            RuleFor(n => n.Description).NotEmpty().WithMessage("Description cannot be blank");
            RuleFor(n => n.Subtitle1).NotEmpty().WithMessage("Subtitle1 cannot be blank");
            RuleFor(n => n.Subtitle2).NotEmpty().WithMessage("Subtitle2 cannot be blank");
        }
    }
}
