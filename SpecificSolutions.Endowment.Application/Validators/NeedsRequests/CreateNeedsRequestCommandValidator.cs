using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.NeedsRequests
{
    public class CreateNeedsRequestCommandValidator : AbstractValidator<CreateNeedsRequestCommand>
    {
        public CreateNeedsRequestCommandValidator()
        {
            RuleFor(x => x.NeedsType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.EstimatedCost).GreaterThan(0);
            RuleFor(x => x.Provider).NotEmpty().MaximumLength(100);
        }
    }
}