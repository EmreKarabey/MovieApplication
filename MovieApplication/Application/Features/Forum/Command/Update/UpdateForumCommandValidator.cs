using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Forum.Command.Update
{
    public class UpdateForumCommandValidator : AbstractValidator<UpdateForumCommand>
    {
        public UpdateForumCommandValidator()
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("Title cannot be blank");
            RuleFor(n => n.Details).NotEmpty().WithMessage("Details cannot be blank");
            RuleFor(n => n.ForumCategoryId).NotEmpty().WithMessage("ForumCategoryId cannot be blank");
        }
    }
}
