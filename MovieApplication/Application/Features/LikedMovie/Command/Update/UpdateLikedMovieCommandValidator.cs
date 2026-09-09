using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.LikedMovie.Command.Update
{
    public class UpdateLikedMovieCommandValidator : AbstractValidator<UpdateLikedMovieCommand>
    {
        public UpdateLikedMovieCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
