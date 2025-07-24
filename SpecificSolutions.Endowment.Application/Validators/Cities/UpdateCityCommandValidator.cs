using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Cities
{
    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("City ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("City name is required.")
                .MaximumLength(200)
                .WithMessage("City name cannot exceed 200 characters.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(100)
                .WithMessage("Country name cannot exceed 100 characters.");
        }
    }
} 