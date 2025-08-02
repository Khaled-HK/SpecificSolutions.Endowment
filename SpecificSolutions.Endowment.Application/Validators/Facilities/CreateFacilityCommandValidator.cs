using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Facilities
{
    public class CreateFacilityCommandValidator : BaseValidator<CreateFacilityCommand>
    {
        public CreateFacilityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المنشأة مطلوب")
                .MaximumLength(200).WithMessage("اسم المنشأة يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المنشأة يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("موقع المنشأة مطلوب")
                .MaximumLength(200).WithMessage("موقع المنشأة يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("موقع المنشأة يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.ContactInfo)
                .NotEmpty().WithMessage("معلومات الاتصال مطلوبة")
                .MaximumLength(100).WithMessage("معلومات الاتصال يجب أن لا تتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("معلومات الاتصال تحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage("السعة يجب أن تكون أكبر من أو تساوي صفر");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("حالة المنشأة مطلوبة")
                .MaximumLength(50).WithMessage("حالة المنشأة يجب أن لا تتجاوز 50 حرف")
                .Must(BeValidName).WithMessage("حالة المنشأة تحتوي على أحرف غير مسموحة");
        }
    }
}