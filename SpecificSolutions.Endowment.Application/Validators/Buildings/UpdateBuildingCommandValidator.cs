using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Buildings
{
    public class UpdateBuildingCommandValidator : AbstractValidator<UpdateBuildingCommand>
    {
        public UpdateBuildingCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            // Other rules...
        }
    }
} 