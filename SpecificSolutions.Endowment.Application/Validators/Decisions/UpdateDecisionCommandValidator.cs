using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Decisions
{
    public class UpdateDecisionCommandValidator : BaseValidator<UpdateDecisionCommand>
    {
        public UpdateDecisionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف القرار مطلوب");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان القرار مطلوب")
                .MaximumLength(200).WithMessage("عنوان القرار يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("عنوان القرار يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("وصف القرار مطلوب")
                .MaximumLength(1000).WithMessage("وصف القرار يجب أن لا يتجاوز 1000 حرف")
                .Must(BeValidName).WithMessage("وصف القرار يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("رقم المرجع مطلوب")
                .MaximumLength(50).WithMessage("رقم المرجع يجب أن لا يتجاوز 50 حرف")
                .Must(BeValidName).WithMessage("رقم المرجع يحتوي على أحرف غير مسموحة");
        }
    }
}