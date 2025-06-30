using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;

public class CreateDecisionCommandValidator : AbstractValidator<CreateDecisionCommand>
{
    public CreateDecisionCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("Created date is required.");
        RuleFor(x => x.ReferenceNumber).NotEmpty().WithMessage("Reference number is required.");
    }
}