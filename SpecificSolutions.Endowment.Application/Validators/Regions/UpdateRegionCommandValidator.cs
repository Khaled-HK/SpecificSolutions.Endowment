using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Regions
{
    public class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Region ID is required.");

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