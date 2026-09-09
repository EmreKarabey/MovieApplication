using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Create;
using FluentValidation;

namespace Application.Features.UnlikedMovie.Commands.Create
{
    public class CreatedUnlikedMovieCommandValidator : AbstractValidator<CreatedUnlikedMovieCommand>
    {
        public CreatedUnlikedMovieCommandValidator()
        {
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
