using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Category.Command.Create
{
    public class CreatedCategoryCommandValidatior : AbstractValidator<CreatedCategoryCommand>
    {
        public CreatedCategoryCommandValidatior()
        {
            RuleFor(n => n.CategoryName).NotEmpty().WithMessage("Category Name cannot be blank");
        }
    }
}
