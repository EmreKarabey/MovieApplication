using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Comment.Command.Delete
{
    public class DeletedCommentCommandValidator : AbstractValidator<DeletedCommentCommand>
    {
        public DeletedCommentCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
