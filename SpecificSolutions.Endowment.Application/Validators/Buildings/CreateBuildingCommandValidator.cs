using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.Buildings
{
    public class CreateBuildingCommandValidator : BaseValidator<CreateBuildingCommand>
    {
        public CreateBuildingCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المبنى مطلوب")
                .MaximumLength(200).WithMessage("اسم المبنى يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المبنى يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.FileNumber)
                .NotEmpty().WithMessage("رقم الملف مطلوب")
                .MaximumLength(50).WithMessage("رقم الملف يجب أن لا يتجاوز 50 حرف");

            RuleFor(x => x.Definition)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Definition))
                .WithMessage("التعريف يجب أن لا يتجاوز 500 حرف");

            RuleFor(x => x.Classification)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Classification))
                .WithMessage("التصنيف يجب أن لا يتجاوز 100 حرف");

            RuleFor(x => x.OfficeId)
                .NotEmpty().WithMessage("المكتب مطلوب")
                .Must(BeValidGuid).WithMessage("معرف المكتب غير صحيح");

            RuleFor(x => x.Unit)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Unit))
                .WithMessage("الوحدة يجب أن لا تتجاوز 100 حرف");

            RuleFor(x => x.RegionId)
                .NotEmpty().WithMessage("المنطقة مطلوبة")
                .Must(BeValidGuid).WithMessage("معرف المنطقة غير صحيح");

            RuleFor(x => x.NearestLandmark)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NearestLandmark))
                .WithMessage("أقرب معلم يجب أن لا يتجاوز 200 حرف");

            RuleFor(x => x.ConstructionDate)
                .NotEmpty().WithMessage("تاريخ البناء مطلوب")
                .Must(BeNotInFuture).WithMessage("تاريخ البناء لا يمكن أن يكون في المستقبل")
                .Must(BeNotTooOld).WithMessage("تاريخ البناء غير صحيح");

            RuleFor(x => x.OpeningDate)
                .NotEmpty().WithMessage("تاريخ الافتتاح مطلوب")
                .Must(BeNotInFuture).WithMessage("تاريخ الافتتاح لا يمكن أن يكون في المستقبل")
                .Must(BeNotTooOld).WithMessage("تاريخ الافتتاح غير صحيح")
                .GreaterThanOrEqualTo(x => x.ConstructionDate).WithMessage("تاريخ الافتتاح يجب أن يكون بعد تاريخ البناء");

            RuleFor(x => x.MapLocation)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.MapLocation))
                .WithMessage("موقع الخريطة يجب أن لا يتجاوز 500 حرف");

            RuleFor(x => x.TotalLandArea)
                .GreaterThan(0).WithMessage("إجمالي مساحة الأرض يجب أن تكون أكبر من صفر");

            RuleFor(x => x.TotalCoveredArea)
                .GreaterThan(0).WithMessage("إجمالي المساحة المغطاة يجب أن تكون أكبر من صفر")
                .LessThanOrEqualTo(x => x.TotalLandArea).WithMessage("المساحة المغطاة يجب أن تكون أقل من أو تساوي مساحة الأرض");

            RuleFor(x => x.NumberOfFloors)
                .GreaterThan(0).WithMessage("عدد الطوابق يجب أن يكون أكبر من صفر")
                .LessThanOrEqualTo(50).WithMessage("عدد الطوابق يجب أن لا يتجاوز 50");

            RuleFor(x => x.ElectricityMeter)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.ElectricityMeter))
                .WithMessage("عداد الكهرباء يجب أن لا يتجاوز 100 حرف");

            RuleFor(x => x.AlternativeEnergySource)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.AlternativeEnergySource))
                .WithMessage("مصدر الطاقة البديل يجب أن لا يتجاوز 200 حرف");

            RuleFor(x => x.WaterSource)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.WaterSource))
                .WithMessage("مصدر المياه يجب أن لا يتجاوز 200 حرف");

            RuleFor(x => x.Sanitation)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Sanitation))
                .WithMessage("الصرف الصحي يجب أن لا يتجاوز 200 حرف");

            RuleFor(x => x.BriefDescription)
                .MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.BriefDescription))
                .WithMessage("الوصف المختصر يجب أن لا يتجاوز 1000 حرف");

            RuleFor(x => x.LandDonorName)
                .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.LandDonorName))
                .WithMessage("اسم متبرع الأرض يجب أن لا يتجاوز 200 حرف");

            RuleFor(x => x.PrayerCapacity)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.PrayerCapacity))
                .WithMessage("سعة الصلاة يجب أن لا تتجاوز 100 حرف");

            RuleFor(x => x.SourceFunds)
                .IsInEnum().WithMessage("مصدر التمويل غير صحيح");

            RuleFor(x => x.PicturePath)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.PicturePath))
                .WithMessage("مسار الصورة يجب أن لا يتجاوز 500 حرف");
        }
    }
} 