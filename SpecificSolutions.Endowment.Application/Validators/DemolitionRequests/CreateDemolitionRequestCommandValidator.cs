using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.DemolitionRequests
{
    public class CreateDemolitionRequestCommandValidator : AbstractValidator<CreateDemolitionRequestCommand>
    {
        public CreateDemolitionRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
            RuleFor(x => x.EstimatedCost).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EstimatedTime).NotEmpty().MaximumLength(100);
        }
    }
}