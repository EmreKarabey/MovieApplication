using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Movies.Commands.Delete
{
    public class DeleteMovieCommandValidator : AbstractValidator<DeletedMovieCommand>
    {
        public DeleteMovieCommandValidator()
        {
            RuleFor(n => n.EntityID).NotEmpty().WithMessage("ID Boş Geçilemez !");
        }
    }
}
