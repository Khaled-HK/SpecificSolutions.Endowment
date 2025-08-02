using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Accounts
{
    public class CreateAccountCommandValidator : BaseValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Messages.AccountNameRequired)
                .MaximumLength(200).WithMessage(Messages.AccountNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.AccountNameInvalidCharacters);

            RuleFor(x => x.MotherName)
                .NotEmpty().WithMessage(Messages.MotherNameRequired)
                .MaximumLength(200).WithMessage(Messages.MotherNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.MotherNameInvalidCharacters);

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage(Messages.BirthDateRequired)
                .Must(BeNotInFuture).WithMessage(Messages.BirthDateNotInFuture)
                .Must(BeNotTooOld).WithMessage(Messages.BirthDateInvalid);

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage(Messages.GenderInvalid);

            RuleFor(x => x.Barcode)
                .NotEmpty().WithMessage(Messages.BarcodeRequired)
                .MaximumLength(50).WithMessage(Messages.BarcodeMaxLength);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage(Messages.StatusInvalid);

            RuleFor(x => x.LockerFileNumber)
                .GreaterThan(0).WithMessage(Messages.LockerFileNumberGreaterThanZero);

            RuleFor(x => x.SocialStatus)
                .IsInEnum().WithMessage(Messages.SocialStatusInvalid);

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage(Messages.AccountNumberRequired)
                .MaximumLength(50).WithMessage(Messages.AccountNumberMaxLength);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage(Messages.AccountTypeInvalid);

            RuleFor(x => x.NID)
                .GreaterThan(0).WithMessage(Messages.NIDGreaterThanZero);

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage(Messages.BalanceNotNegative);

            RuleFor(x => x.Note)
                .MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Note))
                .WithMessage(Messages.NoteMaxLength);

            RuleFor(x => x.BookNumber)
                .GreaterThanOrEqualTo(0).WithMessage(Messages.BookNumberNotNegative);

            RuleFor(x => x.PaperNumber)
                .GreaterThanOrEqualTo(0).WithMessage(Messages.PaperNumberNotNegative);

            RuleFor(x => x.RegistrationNumber)
                .GreaterThanOrEqualTo(0).WithMessage(Messages.RegistrationNumberNotNegative);

            RuleFor(x => x.Address)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Address))
                .WithMessage(Messages.AddressMaxLength);

            RuleFor(x => x.City)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.City))
                .WithMessage(Messages.CityMaxLength);

            RuleFor(x => x.Country)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Country))
                .WithMessage(Messages.CountryMaxLength);

            RuleFor(x => x.ContactNumber)
                .Must(phoneNumber => BeValidPhoneNumber(phoneNumber!)).When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
                .WithMessage(Messages.ContactNumberInvalid);

            RuleFor(x => x.Floors)
                .GreaterThanOrEqualTo(0).WithMessage(Messages.FloorsNotNegative)
                .LessThanOrEqualTo(100).WithMessage(Messages.FloorsMaxValue);
        }
    }
}