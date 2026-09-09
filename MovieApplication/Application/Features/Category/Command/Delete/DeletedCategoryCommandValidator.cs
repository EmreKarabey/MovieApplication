using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Category.Command.Delete
{
    public class DeletedCategoryCommandValidator : AbstractValidator<DeletedCategoryCommand>
    {
        public DeletedCategoryCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
