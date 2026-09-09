using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.LikedMovie.Command.Create
{
    public class CreatedLikedMovieCommandValidator : AbstractValidator<CreatedLikedMovieCommand>
    {
        public CreatedLikedMovieCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
