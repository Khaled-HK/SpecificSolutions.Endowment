using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Accounts
{
    public class CreateAccountCommandValidator : BaseValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الحساب مطلوب")
                .MaximumLength(200).WithMessage("اسم الحساب يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم الحساب يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.MotherName)
                .NotEmpty().WithMessage("اسم الأم مطلوب")
                .MaximumLength(200).WithMessage("اسم الأم يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم الأم يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("تاريخ الميلاد مطلوب")
                .Must(BeNotInFuture).WithMessage("تاريخ الميلاد لا يمكن أن يكون في المستقبل")
                .Must(BeNotTooOld).WithMessage("تاريخ الميلاد غير صحيح");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("الجنس غير صحيح");

            RuleFor(x => x.Barcode)
                .NotEmpty().WithMessage("الباركود مطلوب")
                .MaximumLength(50).WithMessage("الباركود يجب أن لا يتجاوز 50 حرف");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("الحالة غير صحيحة");

            RuleFor(x => x.LockerFileNumber)
                .GreaterThan(0).WithMessage("رقم ملف الخزانة يجب أن يكون أكبر من صفر");

            RuleFor(x => x.SocialStatus)
                .IsInEnum().WithMessage("الحالة الاجتماعية غير صحيحة");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("رقم الحساب مطلوب")
                .MaximumLength(50).WithMessage("رقم الحساب يجب أن لا يتجاوز 50 حرف");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("نوع الحساب غير صحيح");

            RuleFor(x => x.NID)
                .GreaterThan(0).WithMessage("الرقم الوطني يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("الرصيد لا يمكن أن يكون سالب");

            RuleFor(x => x.Note)
                .MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Note))
                .WithMessage("الملاحظة يجب أن لا تتجاوز 1000 حرف");

            RuleFor(x => x.BookNumber)
                .GreaterThanOrEqualTo(0).WithMessage("رقم الكتاب لا يمكن أن يكون سالب");

            RuleFor(x => x.PaperNumber)
                .GreaterThanOrEqualTo(0).WithMessage("رقم الورقة لا يمكن أن يكون سالب");

            RuleFor(x => x.RegistrationNumber)
                .GreaterThanOrEqualTo(0).WithMessage("رقم التسجيل لا يمكن أن يكون سالب");

            RuleFor(x => x.Address)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Address))
                .WithMessage("العنوان يجب أن لا يتجاوز 500 حرف");

            RuleFor(x => x.City)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.City))
                .WithMessage("المدينة يجب أن لا تتجاوز 100 حرف");

            RuleFor(x => x.Country)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Country))
                .WithMessage("البلد يجب أن لا يتجاوز 100 حرف");

            RuleFor(x => x.ContactNumber)
                .Must(phoneNumber => BeValidPhoneNumber(phoneNumber!)).When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
                .WithMessage("رقم الاتصال غير صحيح");

            RuleFor(x => x.Floors)
                .GreaterThanOrEqualTo(0).WithMessage("عدد الطوابق لا يمكن أن يكون سالب")
                .LessThanOrEqualTo(100).WithMessage("عدد الطوابق لا يمكن أن يتجاوز 100");
        }
    }
}