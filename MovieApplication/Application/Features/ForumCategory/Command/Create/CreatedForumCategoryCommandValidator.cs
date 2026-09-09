using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.ForumCategory.Command.Create
{
    public class CreatedForumCategoryCommandValidator : AbstractValidator<CreatedForumCategoryCommand>
    {
        public CreatedForumCategoryCommandValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Name cannot be blank");
        }
    }
}
