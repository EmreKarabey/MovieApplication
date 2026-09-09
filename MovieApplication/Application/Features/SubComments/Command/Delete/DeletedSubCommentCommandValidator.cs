using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SubComments.Command.Delete
{
    public class DeletedSubCommentCommandValidator : AbstractValidator<DeletedSubCommentCommand>
    {
        public DeletedSubCommentCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}




