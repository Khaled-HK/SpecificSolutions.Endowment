using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Decisions
{
    public class CreateDecisionCommandValidator : BaseValidator<CreateDecisionCommand>
    {
        public CreateDecisionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(Messages.DecisionTitleRequired)
                .MaximumLength(200).WithMessage(Messages.DecisionTitleMaxLength)
                .Must(BeValidName).WithMessage(Messages.DecisionTitleInvalidCharacters);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(Messages.DecisionDescriptionRequired)
                .MaximumLength(1000).WithMessage(Messages.DecisionDescriptionMaxLength)
                .Must(BeValidName).WithMessage(Messages.DecisionDescriptionInvalidCharacters);

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage(Messages.ReferenceNumberRequired)
                .MaximumLength(50).WithMessage(Messages.ReferenceNumberMaxLength)
                .Must(BeValidName).WithMessage(Messages.ReferenceNumberInvalidCharacters);

            RuleFor(x => x.CreatedDate)
                .NotEmpty().WithMessage(Messages.CreatedDateRequired);
        }
    }
}