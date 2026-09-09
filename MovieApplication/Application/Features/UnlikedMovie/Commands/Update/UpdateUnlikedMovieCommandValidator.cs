using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.UnlikedMovie.Commands.Update
{
    public class UpdateUnlikedMovieCommandValidator : AbstractValidator<UpdateUnlikedMovieCommand>
    {
        public UpdateUnlikedMovieCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
