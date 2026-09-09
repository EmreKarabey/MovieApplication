using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SavedMovie.Command.DeleteIdMovie
{
    public class DeleteIdMovieCommandValidator : AbstractValidator<DeleteIdMovieCommand>
    {
        public DeleteIdMovieCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieId cannot be blank");
            RuleFor(n => n.UserID).NotEmpty().WithMessage("UserId cannot be blank");
        }
    }
}
