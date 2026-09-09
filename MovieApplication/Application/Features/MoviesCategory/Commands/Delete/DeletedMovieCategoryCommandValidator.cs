using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.MoviesCategory.Commands.Delete
{
    public class DeletedMovieCategoryCommandValidator : AbstractValidator<DeletedMovieCategoryCommand>
    {
        public DeletedMovieCategoryCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
