using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;

public class UpdateDecisionCommandValidator : AbstractValidator<UpdateDecisionCommand>
{
    public UpdateDecisionCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ReferenceNumber).NotEmpty().WithMessage("Reference number is required.");
    }
}