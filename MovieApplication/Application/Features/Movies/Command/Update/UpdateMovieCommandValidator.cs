using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Movies.Commands.Update
{
    public class UpdateMovieCommandValidator : AbstractValidator<UpdatedMovieCommand>
    {
        public UpdateMovieCommandValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("Name cannot be blank").MinimumLength(2).WithMessage("Name must be minimum three characters.");
            RuleFor(n => n.ProducerName).NotEmpty().WithMessage("Producer Name cannot be blank.");
            RuleFor(n => n.Description).NotEmpty().WithMessage("Description cannot be blank.");
            RuleFor(n => n.ReleaseDate).NotEmpty().WithMessage("ReleaseDate cannot be blank.");
        }
    }
}
