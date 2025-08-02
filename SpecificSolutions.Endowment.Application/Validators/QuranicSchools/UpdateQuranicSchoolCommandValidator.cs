using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Validators.QuranicSchools
{
    public class UpdateQuranicSchoolCommandValidator : BaseValidator<UpdateQuranicSchoolCommand>
    {
        public UpdateQuranicSchoolCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المدرسة القرآنية مطلوب")
                .Must(BeValidGuid).WithMessage("معرف المدرسة القرآنية غير صحيح");

            RuleFor(x => x.SchoolName)
                .NotEmpty().WithMessage("اسم المدرسة القرآنية مطلوب")
                .MaximumLength(200).WithMessage("اسم المدرسة القرآنية يجب أن لا يتجاوز 200 حرف")
                .Must(BeValidName).WithMessage("اسم المدرسة القرآنية يحتوي على أحرف غير مسموحة");

            RuleFor(x => x.Building)
                .NotNull().WithMessage("بيانات المبنى مطلوبة");
        }
    }
} 