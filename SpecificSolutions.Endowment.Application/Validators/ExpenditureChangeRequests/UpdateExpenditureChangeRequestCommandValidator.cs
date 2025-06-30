using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.EndowmentExpenditureChangeRequests
{
    public class UpdateExpenditureChangeRequestCommandValidator : AbstractValidator<UpdateExpenditureChangeRequestCommand>
    {
        public UpdateExpenditureChangeRequestCommandValidator()
        {
            RuleFor(x => x.RequestStatus).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CurrentExpenditure).NotEmpty().MaximumLength(500);
            RuleFor(x => x.NewExpenditure).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Reason).MaximumLength(500);
        }
    }
}