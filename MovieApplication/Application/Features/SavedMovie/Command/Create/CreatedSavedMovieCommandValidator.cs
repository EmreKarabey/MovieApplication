using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SavedMovie.Command.Create
{
    public class CreatedSavedMovieCommandValidator : AbstractValidator<CreatedSavedMovieCommand>
    {
        public CreatedSavedMovieCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
