using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Offices
{
    public class UpdateOfficeCommandValidator : BaseValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المكتب مطلوب");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المكتب مطلوب")
                .MaximumLength(200).WithMessage("اسم المكتب يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المكتب يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("رقم الهاتف مطلوب")
                .Must(BeValidPhoneNumber).WithMessage("رقم الهاتف غير صحيح");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("موقع المكتب مطلوب")
                .MaximumLength(500).WithMessage("موقع المكتب يجب أن لا يتجاوز 500 حرف")
                .Must(BeValidName).WithMessage("موقع المكتب يحتوي على أحرف غير مسموحة");
        }
    }
}