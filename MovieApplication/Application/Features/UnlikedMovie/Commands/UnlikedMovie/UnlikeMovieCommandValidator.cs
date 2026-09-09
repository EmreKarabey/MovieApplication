using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.UnlikedMovie.Commands.UnlikedMovie
{
    public class UnlikeMovieCommandValidator : AbstractValidator<UnlikeMovieCommand>
    {
        public UnlikeMovieCommandValidator()
        {
            RuleFor(n => n.MovieId).NotEmpty().WithMessage("MovieId cannot be blank");
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be blank");
        }
    }
}
