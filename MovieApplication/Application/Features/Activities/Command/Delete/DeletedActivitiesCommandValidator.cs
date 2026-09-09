using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Activities.Command.Delete
{
    public class DeletedActivitiesCommandValidator : AbstractValidator<DeletedActivitiesCommand>
    {
        public DeletedActivitiesCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
