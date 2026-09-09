using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.LikedMovie.Command.Delete;
using FluentValidation;

namespace Application.Features.History.Command.Delete
{
    public class DeletedHistoryCommandValidator : AbstractValidator<DeletedHistoryCommand>
    {
        public DeletedHistoryCommandValidator()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
        }
    }
}
