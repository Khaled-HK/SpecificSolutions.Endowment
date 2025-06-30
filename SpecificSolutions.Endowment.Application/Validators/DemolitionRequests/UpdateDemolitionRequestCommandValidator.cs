using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.DemolitionRequests
{
    public class UpdateDemolitionRequestCommandValidator : AbstractValidator<UpdateDemolitionRequestCommand>
    {
        public UpdateDemolitionRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
            RuleFor(x => x.EstimatedCost).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EstimatedTime).NotEmpty().MaximumLength(100);
        }
    }
}