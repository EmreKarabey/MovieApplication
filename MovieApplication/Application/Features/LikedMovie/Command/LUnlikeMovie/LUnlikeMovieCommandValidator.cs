using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.LikedMovie.Command.UnlikeMovie
{
    public class LUnlikeMovieCommandValidator : AbstractValidator<LUnlikeMovieCommand>
    {
        public LUnlikeMovieCommandValidator()
        {
            RuleFor(n => n.MovieId).NotEmpty().WithMessage("MovieId cannot be blank");
            RuleFor(n => n.UserId).NotEmpty().WithMessage("UserId cannot be blank");
        }
    }
}
