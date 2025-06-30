using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.ChangeOfPathRequests
{
    public class UpdateChangeOfPathRequestCommandValidator : AbstractValidator<UpdateChangeOfPathRequestCommand>
    {
        public UpdateChangeOfPathRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CurrentType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.NewType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Reason).MaximumLength(500);
        }
    }
}