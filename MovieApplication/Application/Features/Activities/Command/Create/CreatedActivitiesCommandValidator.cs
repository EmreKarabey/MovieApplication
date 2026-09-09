using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Activities.Command.Create
{
    public class CreatedActivitiesCommandValidator : AbstractValidator<CreatedActivitiesCommand>
    {
        public CreatedActivitiesCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
            RuleFor(n => n.ActivitiesCategory).NotEmpty().WithMessage("ActivitiesCategory cannot be blank");
        }
    }
}
