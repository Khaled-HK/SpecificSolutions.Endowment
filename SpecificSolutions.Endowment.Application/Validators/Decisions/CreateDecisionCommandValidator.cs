using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;

public class CreateDecisionCommandValidator : AbstractValidator<CreateDecisionCommand>
{
    public CreateDecisionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان القرار مطلوب.")
            .MaximumLength(200)
            .WithMessage("عنوان القرار يجب أن لا يتجاوز 200 حرف.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("وصف القرار مطلوب.")
            .MaximumLength(1000)
            .WithMessage("وصف القرار يجب أن لا يتجاوز 1000 حرف.");

        RuleFor(x => x.CreatedDate)
            .NotEmpty()
            .WithMessage("تاريخ الإنشاء مطلوب.");

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .WithMessage("رقم المرجع مطلوب.")
            .MaximumLength(50)
            .WithMessage("رقم المرجع يجب أن لا يتجاوز 50 حرف.");

        //RuleFor(x => x.UserId)
        //    .NotEmpty()
        //    .WithMessage("معرف المستخدم مطلوب.");
    }
}