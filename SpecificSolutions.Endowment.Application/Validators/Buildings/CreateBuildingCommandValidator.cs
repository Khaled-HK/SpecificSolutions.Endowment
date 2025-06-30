using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Buildings
{
    public class CreateBuildingCommandValidator : AbstractValidator<CreateBuildingCommand>
    {
        public CreateBuildingCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            // Other rules...
        }
    }
} 