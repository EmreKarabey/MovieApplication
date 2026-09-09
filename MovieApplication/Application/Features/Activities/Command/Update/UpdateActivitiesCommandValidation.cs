using FluentValidation;

namespace Application.Features.Activities.Command.Update
{
    public class UpdateActivitiesCommandValidation : AbstractValidator<UpdateActivitiesCommand>
    {
        public UpdateActivitiesCommandValidation()
        {
            RuleFor(n => n.Id).NotEmpty().WithMessage("Id cannot be blank");
            RuleFor(n => n.UserID).NotEmpty().WithMessage("UserID cannot be blank");
            RuleFor(n => n.MovieID).NotEmpty().WithMessage("MovieID cannot be blank");
            RuleFor(n => n.ActivitiesCategory).IsInEnum().WithMessage("Invalid category");
        }
    }
}
