using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.MoviesCategory.Commands.Update
{
    public class UpdatedMovieCategoryCommandValidation : AbstractValidator<UpdatedMovieCategoryCommand>
    {
        public UpdatedMovieCategoryCommandValidation()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
            RuleFor(n => n.CategoryID).NotEmpty().WithMessage("CategoryID cannot be blank");
        }
    }
}
