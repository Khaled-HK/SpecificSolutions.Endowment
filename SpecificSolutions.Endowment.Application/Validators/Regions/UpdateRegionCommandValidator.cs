using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.Regions
{
    public class UpdateRegionCommandValidator : BaseValidator<UpdateRegionCommand>
    {
        public UpdateRegionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المنطقة مطلوب");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم المنطقة مطلوب")
                .MaximumLength(200).WithMessage("اسم المنطقة يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المنطقة يحتوي على أحرف غير مسموحة");
        }
    }
} 