using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Decisions
{
    public class UpdateDecisionCommandValidator : AbstractValidator<UpdateDecisionCommand>
    {
        public UpdateDecisionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("عنوان القرار مطلوب");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("وصف القرار مطلوب");

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty()
                .WithMessage("رقم المرجع مطلوب");
        }
    }
}