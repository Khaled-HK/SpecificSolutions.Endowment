using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Regions
{
    public class CreateRegionCommandValidator : BaseValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Messages.RegionNameRequired)
                .MaximumLength(200).WithMessage(Messages.RegionNameMaxLength)
                .Must(BeValidName).WithMessage(Messages.RegionNameInvalidCharacters);
        }
    }
} 