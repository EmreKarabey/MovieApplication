using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.LikedMovie.Command.Delete
{
    public class DeletedUnlikedMovieCommandValidator : AbstractValidator<DeletedUnlikedMovieCommand>
    {
        public DeletedUnlikedMovieCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }

    }
}
