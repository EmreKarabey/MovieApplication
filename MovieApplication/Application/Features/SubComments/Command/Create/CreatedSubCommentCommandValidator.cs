using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SubComments.Command.Create
{
    public class CreatedSubCommentCommandValidator : AbstractValidator<CreatedSubCommentCommand>
    {
        public CreatedSubCommentCommandValidator()
        {
            RuleFor(n => n.Content).NotEmpty().WithMessage("Content cannot be blank");
            RuleFor(n => n.CommentID).NotEmpty().WithMessage("CommentID cannot be blank");
        }
    }
}




