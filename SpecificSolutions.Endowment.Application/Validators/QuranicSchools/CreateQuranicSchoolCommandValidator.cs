using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.QuranicSchools
{
    public class CreateQuranicSchoolCommandValidator : BaseValidator<CreateQuranicSchoolCommand>
    {
        public CreateQuranicSchoolCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المدرسة القرآنية مطلوب")
                .MaximumLength(200).WithMessage("اسم المدرسة القرآنية يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المدرسة القرآنية يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.FileNumber)
                .NotEmpty().WithMessage("رقم الملف مطلوب")
                .MaximumLength(50).WithMessage("رقم الملف يجب أن لا يتجاوز 50 حرف")
                .Must(BeValidName).WithMessage("رقم الملف يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Definition)
                .NotEmpty().WithMessage("التعريف مطلوب")
                .MaximumLength(500).WithMessage("التعريف يجب أن لا يتجاوز 500 حرف")
                .Must(BeValidName).WithMessage("التعريف يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Classification)
                .NotEmpty().WithMessage("التصنيف مطلوب")
                .MaximumLength(100).WithMessage("التصنيف يجب أن لا يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("التصنيف يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.OfficeId)
                .NotEmpty().WithMessage("معرف المكتب مطلوب")
                .Must(BeValidGuid).WithMessage("معرف المكتب غير صحيح");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("الوحدة مطلوبة")
                .MaximumLength(100).WithMessage("الوحدة يجب أن لا تتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("الوحدة تحتوي على أحرف غير مسموحة");

            RuleFor(x => x.RegionId)
                .NotEmpty().WithMessage("معرف المنطقة مطلوب")
                .Must(BeValidGuid).WithMessage("معرف المنطقة غير صحيح");

            RuleFor(x => x.NearestLandmark)
                .NotEmpty().WithMessage("أقرب معلم مطلوب")
                .MaximumLength(200).WithMessage("أقرب معلم يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("أقرب معلم يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.MapLocation)
                .NotEmpty().WithMessage("موقع الخريطة مطلوب")
                .MaximumLength(500).WithMessage("موقع الخريطة يجب أن لا يتجاوز 500 حرف")
                .Must(BeValidName).WithMessage("موقع الخريطة يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.ElectricityMeter)
                .NotEmpty().WithMessage("عداد الكهرباء مطلوب")
                .MaximumLength(100).WithMessage("عداد الكهرباء يجب أن لا يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("عداد الكهرباء يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.AlternativeEnergySource)
                .NotEmpty().WithMessage("مصدر الطاقة البديلة مطلوب")
                .MaximumLength(100).WithMessage("مصدر الطاقة البديلة يجب أن لا يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("مصدر الطاقة البديلة يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.WaterSource)
                .NotEmpty().WithMessage("مصدر المياه مطلوب")
                .MaximumLength(100).WithMessage("مصدر المياه يجب أن لا يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("مصدر المياه يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Sanitation)
                .NotEmpty().WithMessage("الصرف الصحي مطلوب")
                .MaximumLength(100).WithMessage("الصرف الصحي يجب أن لا يتجاوز 100 حرف")
                .Must(BeValidName).WithMessage("الصرف الصحي يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.BriefDescription)
                .NotEmpty().WithMessage("الوصف المختصر مطلوب")
                .MaximumLength(1000).WithMessage("الوصف المختصر يجب أن لا يتجاوز 1000 حرف")
                .Must(BeValidName).WithMessage("الوصف المختصر يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("معرف المستخدم مطلوب")
                .Must(BeValidGuid).WithMessage("معرف المستخدم غير صحيح");

            RuleFor(x => x.PicturePath)
                .NotEmpty().WithMessage("مسار الصورة مطلوب")
                .MaximumLength(500).WithMessage("مسار الصورة يجب أن لا يتجاوز 500 حرف")
                .Must(BeValidName).WithMessage("مسار الصورة يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.LandDonorName)
                .NotEmpty().WithMessage("اسم متبرع الأرض مطلوب")
                .MaximumLength(200).WithMessage("اسم متبرع الأرض يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم متبرع الأرض يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.PrayerCapacity)
                .NotEmpty().WithMessage("سعة الصلاة مطلوبة")
                .MaximumLength(50).WithMessage("سعة الصلاة يجب أن لا تتجاوز 50 حرف")
                .Must(BeValidName).WithMessage("سعة الصلاة تحتوي على أحرف غير مسموحة");
        }
    }
}