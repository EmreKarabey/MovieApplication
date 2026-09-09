using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SavedMovie.Command.Delete
{
    public class DeletedSavedMovieCommandValidator : AbstractValidator<DeletedSavedMovieCommand>
    {
        public DeletedSavedMovieCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
