using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Facilities
{
    public class CreateFacilityCommandValidator : AbstractValidator<CreateFacilityCommand>
    {
        public CreateFacilityCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ContactInfo).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Capacity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}