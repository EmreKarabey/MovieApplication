using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(n => n.FirstName).NotEmpty().WithMessage("FirstName cannot be left blank").MinimumLength(3).WithMessage("Minimum name must be 3 characters.");
            RuleFor(n => n.LastName).NotEmpty().WithMessage("Surname cannot be left blank.");
            RuleFor(n => n.Email).NotEmpty().WithMessage("Email cannot be left blank.").EmailAddress().WithMessage("Invalid email format.");
            RuleFor(n => n.Password).NotEmpty().WithMessage("Password cannot be left blank.");
        }
    }
}
