using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.History.Command.Update
{
    public class UpdateHistoryCommandValidator : AbstractValidator<UpdateHistoryCommand>
    {
        public UpdateHistoryCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be a blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be a blank");
            RuleFor(n => n.TotalSeconds).NotEmpty().WithMessage("TotalSeconds cannot be a blank");
            RuleFor(n => n.WatchedSeconds).NotEmpty().WithMessage("WatchedSeconds cannot be a blank");
        }
    }
}
