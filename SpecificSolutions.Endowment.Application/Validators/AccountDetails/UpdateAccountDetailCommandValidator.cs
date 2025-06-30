using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.AccountDetails
{
    public class UpdateAccountDetailCommandValidator : AbstractValidator<UpdateAccountDetailCommand>
    {
        public UpdateAccountDetailCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Debtor).NotEmpty().WithMessage("Debtor is required.");
            RuleFor(x => x.Creditor).NotEmpty().WithMessage("Creditor is required.");
            RuleFor(x => x.Note).MaximumLength(500).WithMessage("Note cannot exceed 500 characters.");
        }
    }
}
