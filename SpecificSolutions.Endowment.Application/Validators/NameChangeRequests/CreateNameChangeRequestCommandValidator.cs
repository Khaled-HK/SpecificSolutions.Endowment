using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.NameChangeRequests
{
    public class CreateNameChangeRequestCommandValidator : AbstractValidator<CreateNameChangeRequestCommand>
    {
        public CreateNameChangeRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CurrentName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NewName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Reason).MaximumLength(500);
        }
    }
}