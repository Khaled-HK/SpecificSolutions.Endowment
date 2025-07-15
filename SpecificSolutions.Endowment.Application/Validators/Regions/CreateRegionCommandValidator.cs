using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Regions
{
    public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Region name is required.")
                .MaximumLength(100)
                .WithMessage("Region name cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(100)
                .WithMessage("Country name cannot exceed 100 characters.");

            RuleFor(x => x.CityId)
                .NotEmpty()
                .WithMessage("City is required.");
        }
    }
} 