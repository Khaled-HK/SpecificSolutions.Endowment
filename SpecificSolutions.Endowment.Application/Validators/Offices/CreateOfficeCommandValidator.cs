using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Offices
{
    public class CreateOfficeCommandValidator : BaseValidator<CreateOfficeCommand>
    {
        public CreateOfficeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Messages.OfficeNameRequired)
                .MaximumLength(200).WithMessage(Messages.OfficeNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.OfficeNameInvalidCharacters);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(Messages.PhoneNumberRequired)
                .Must(BeValidPhoneNumber).WithMessage(Messages.PhoneNumberInvalid);

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage(Messages.OfficeLocationRequired)
                .MaximumLength(500).WithMessage(Messages.OfficeLocationMaxLength)
                .Must(BeValidName).WithMessage(Messages.OfficeLocationInvalidCharacters);
        }
    }
} 