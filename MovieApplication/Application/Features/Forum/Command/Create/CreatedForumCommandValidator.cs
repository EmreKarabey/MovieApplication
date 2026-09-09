using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Forum.Command.Create
{
    public class CreatedForumCommandValidator : AbstractValidator<CreatedForumCommand>
    {
        public CreatedForumCommandValidator()
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("Title cannot be blank");
            RuleFor(n => n.Details).NotEmpty().WithMessage("Details cannot be blank");
            RuleFor(n => n.ForumCategoryId).NotEmpty().WithMessage("ForumCategoryId cannot be blank");
        }
    }
}
