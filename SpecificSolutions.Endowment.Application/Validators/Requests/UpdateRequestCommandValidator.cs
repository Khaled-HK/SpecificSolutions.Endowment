using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Requests
{
    public class UpdateRequestCommandValidator : BaseValidator<UpdateRequestCommand>
    {
        public UpdateRequestCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الطلب مطلوب")
                .Must(BeValidGuid).WithMessage("معرف الطلب غير صحيح");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان الطلب مطلوب")
                .MaximumLength(100).WithMessage("عنوان الطلب لا يمكن أن يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("عنوان الطلب يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Description)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("وصف الطلب لا يمكن أن يتجاوز 500 حرف")
                .Must(BeValidName).When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("وصف الطلب يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("رقم المرجع مطلوب")
                .MaximumLength(50).WithMessage("رقم المرجع لا يمكن أن يتجاوز 50 حرف")
                .Must(BeValidName).WithMessage("رقم المرجع يحتوي على أحرف غير مسموحة");
        }
    }
} 