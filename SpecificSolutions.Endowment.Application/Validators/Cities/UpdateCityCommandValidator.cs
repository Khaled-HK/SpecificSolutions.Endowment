using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Cities
{
    public class UpdateCityCommandValidator : BaseValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(Messages.CityIdRequired)
                .Must(BeValidGuid).WithMessage(Messages.IdRequired);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Messages.CityNameRequired)
                .MaximumLength(200).WithMessage(Messages.CityNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.CityNameInvalidCharacters);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage(Messages.CityCountryRequired)
                .MaximumLength(100).WithMessage(Messages.CityCountryMaxLength)
                .Must(BeValidName).WithMessage(Messages.CityCountryInvalidCharacters);
        }
    }
} 