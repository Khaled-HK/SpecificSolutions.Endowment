kuusing FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Validators.BuildingDetails
{
    public class CreateBuildingDetailCommandValidator : AbstractValidator<CreateBuildingDetailCommand>
    {
        public CreateBuildingDetailCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المبنى مطلوب")
                .MaximumLength(200).WithMessage("اسم المبنى لا يمكن أن يتجاوز 200 حرف");

            RuleFor(x => x.Floors)
                .GreaterThan(0).WithMessage("عدد الطوابق يجب أن يكون أكبر من صفر")
                .LessThanOrEqualTo(100).WithMessage("عدد الطوابق لا يمكن أن يتجاوز 100");

            RuleFor(x => x.BuildingCategory)
                .IsInEnum().WithMessage("نوع المبنى غير صحيح");

            RuleFor(x => x.BuildingId)
                .NotEmpty().WithMessage("معرف المبنى مطلوب");
        }
    }
} 