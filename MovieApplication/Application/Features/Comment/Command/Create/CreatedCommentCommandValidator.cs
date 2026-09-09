using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Comment.Command.Create
{
    public class CreatedCommentCommandValidator : AbstractValidator<CreatedCommentCommand>
    {
        public CreatedCommentCommandValidator()
        {
            RuleFor(n => n.Content).NotEmpty().WithMessage("Content cannot be blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
        }
    }
}
