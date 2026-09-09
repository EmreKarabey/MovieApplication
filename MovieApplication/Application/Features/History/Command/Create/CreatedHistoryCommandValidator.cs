using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.History.Command.Create
{
    public class CreatedHistoryCommandValidator : AbstractValidator<CreatedHistoryCommand>
    {
        public CreatedHistoryCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be a blank");
        }
    }
}
