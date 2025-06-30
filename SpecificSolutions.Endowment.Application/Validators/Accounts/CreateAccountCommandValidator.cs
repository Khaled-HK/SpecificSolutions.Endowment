using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Accounts
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.MotherName).NotEmpty().WithMessage("Mother's name is required.");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Birth date is required.");
            RuleFor(x => x.Gender).IsInEnum().WithMessage("Gender is required.");
            RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barcode is required.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("Status is required.");
            RuleFor(x => x.LockerFileNumber).GreaterThan(0).WithMessage("Locker file number must be greater than 0.");
            RuleFor(x => x.SocialStatus).IsInEnum().WithMessage("Social status is required.");
            RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required.");
            RuleFor(x => x.Type).IsInEnum().WithMessage("Account type is required.");
            RuleFor(x => x.NID).GreaterThan(0).WithMessage("NID must be greater than 0.");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(0).WithMessage("Balance cannot be negative.");
        }
    }
}