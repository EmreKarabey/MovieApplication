using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.ForumCategory.Command.Delete
{
    public class DeletedForumCategoryCommandValidator : AbstractValidator<DeletedForumCategoryCommand>
    {
        public DeletedForumCategoryCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
