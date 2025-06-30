using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Facilities
{
    public class UpdateFacilityCommandValidator : AbstractValidator<UpdateFacilityCommand>
    {
        public UpdateFacilityCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ContactInfo).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Capacity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}