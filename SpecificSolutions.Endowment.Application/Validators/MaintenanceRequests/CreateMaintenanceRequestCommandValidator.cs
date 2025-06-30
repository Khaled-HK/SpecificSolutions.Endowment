using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.MaintenanceRequests
{
    public class CreateMaintenanceRequestCommandValidator : AbstractValidator<CreateMaintenanceRequestCommand>
    {
        public CreateMaintenanceRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.MaintenanceType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(500);
            RuleFor(x => x.EstimatedCost).GreaterThan(0);
        }
    }
}