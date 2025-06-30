using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.NeedsRequests
{
    public class UpdateNeedsRequestCommandValidator : AbstractValidator<UpdateNeedsRequestCommand>
    {
        public UpdateNeedsRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Priority).NotEmpty().MaximumLength(50);
        }
    }
}