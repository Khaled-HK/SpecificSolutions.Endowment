using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.NeedsRequests
{
    public class CreateNeedsRequestCommandValidator : AbstractValidator<CreateNeedsRequestCommand>
    {
        public CreateNeedsRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Priority).NotEmpty().MaximumLength(50);
        }
    }
}