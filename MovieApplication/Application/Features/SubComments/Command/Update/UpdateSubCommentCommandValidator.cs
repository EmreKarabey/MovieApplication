using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.SubComments.Command.Update
{
    public class UpdateSubCommentCommandValidator : AbstractValidator<UpdateSubCommentCommand>
    {
        public UpdateSubCommentCommandValidator()
        {
            RuleFor(n => n.Content).NotEmpty().WithMessage("Content cannot be blank");
            RuleFor(n => n.CommentID).NotEmpty().WithMessage("CommentID cannot be blank");
        }
    }
}




