using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.AccountDetails
{
    public class UpdateAccountDetailCommandValidator : BaseValidator<UpdateAccountDetailCommand>
    {
        public UpdateAccountDetailCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف تفصيل الحساب مطلوب")
                .Must(BeValidGuid).WithMessage("معرف تفصيل الحساب غير صحيح");

            RuleFor(x => x.Debtor)
                .NotEmpty().WithMessage("المدين مطلوب");

            RuleFor(x => x.Creditor)
                .NotEmpty().WithMessage("الدائن مطلوب");

            RuleFor(x => x.Note)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Note))
                .WithMessage("الملاحظة يجب أن لا تتجاوز 500 حرف")
                .Must(BeValidName).When(x => !string.IsNullOrWhiteSpace(x.Note))
                .WithMessage("الملاحظة تحتوي على أحرف غير مسموحة");
        }
    }
}
