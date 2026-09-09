using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.ForumCategory.Command.Update
{
    public class UpdatedForumCategoryCommandValidator : AbstractValidator<UpdatedForumCategoryCommand>
    {
        public UpdatedForumCategoryCommandValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Name cannot be blank");
        }
    }
}
