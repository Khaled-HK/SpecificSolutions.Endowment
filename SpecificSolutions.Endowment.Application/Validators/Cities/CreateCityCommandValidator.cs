using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Cities
{
    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
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