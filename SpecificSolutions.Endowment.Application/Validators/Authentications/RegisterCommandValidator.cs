using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class RegisterCommandValidator : BaseValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(Messages.FirstNameRequired)
                .MaximumLength(50).WithMessage(Messages.FirstNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.FirstNameInvalidCharacters);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(Messages.LastNameRequired)
                .MaximumLength(50).WithMessage(Messages.LastNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.LastNameInvalidCharacters);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Messages.EmailRequired)
                .EmailAddress().WithMessage(Messages.EmailInvalid)
                .MaximumLength(100).WithMessage(Messages.EmailMaxLength);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(Messages.UserNameRequired)
                .MaximumLength(50).WithMessage(Messages.UserNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.UserNameInvalidCharacters);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Messages.PasswordRequired)
                .MinimumLength(6).WithMessage(Messages.PasswordMinLength)
                .MaximumLength(100).WithMessage(Messages.PasswordMaxLength);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(Messages.ConfirmPasswordRequired)
                .Equal(x => x.Password).WithMessage(Messages.PasswordsDoNotMatch);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(Messages.PhoneNumberRequired)
                .Must(BeValidPhoneNumber).WithMessage(Messages.PhoneNumberInvalid);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(Messages.AddressRequired)
                .MaximumLength(200).WithMessage(Messages.AddressMaxLength)
                .Must(BeValidName).WithMessage(Messages.AddressInvalidCharacters);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage(Messages.CityRequired)
                .MaximumLength(100).WithMessage(Messages.CityMaxLength)
                .Must(BeValidName).WithMessage(Messages.CityInvalidCharacters);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage(Messages.CountryRequired)
                .MaximumLength(100).WithMessage(Messages.CountryMaxLength)
                .Must(BeValidName).WithMessage(Messages.CountryInvalidCharacters);

            RuleFor(x => x.OfficeId)
                .NotEmpty().WithMessage(Messages.OfficeRequired);
        }
    }
}