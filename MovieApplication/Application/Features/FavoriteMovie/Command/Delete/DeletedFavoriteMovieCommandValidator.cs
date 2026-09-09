using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.FavoriteMovie.Command.Delete
{
    public class DeletedFavoriteMovieCommandValidator : AbstractValidator<DeletedFavoriteMovieCommand>
    {
        public DeletedFavoriteMovieCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
