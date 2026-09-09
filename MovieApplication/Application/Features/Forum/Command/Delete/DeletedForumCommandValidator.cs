using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Forum.Command.Delete
{
    public class DeletedForumCommandValidator : AbstractValidator<DeletedForumCommand>
    {
        public DeletedForumCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
