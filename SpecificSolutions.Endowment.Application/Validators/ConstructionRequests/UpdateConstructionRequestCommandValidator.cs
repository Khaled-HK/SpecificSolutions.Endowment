using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.ConstructionRequests
{
    public class UpdateConstructionRequestCommandValidator : AbstractValidator<UpdateConstructionRequestCommand>
    {
        public UpdateConstructionRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.BuildingType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ProposedLocation).NotEmpty().MaximumLength(500);
            RuleFor(x => x.ProposedArea).GreaterThan(0);
            RuleFor(x => x.EstimatedCost).GreaterThan(0);
            RuleFor(x => x.ContractorName).MaximumLength(200);
        }
    }
}