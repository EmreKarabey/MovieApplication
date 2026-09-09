using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.FavoriteMovie.Command.Create
{
    public class CreatedFavoriteMovieCommandValidator : AbstractValidator<CreatedFavoriteMovieResponse>
    {
        public CreatedFavoriteMovieCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
