using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Category.Command.Update
{
    public class UpdatedCategoryCommandValidation : AbstractValidator<UpdatedCategoryCommand>
    {
        public UpdatedCategoryCommandValidation()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.CategoryName).NotEmpty().WithMessage("CategoryName cannot be blank");
        }
    }
}
