using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResendEmailConfirmation;
using SpecificSolutions.Endowment.Application.Validators;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    /// <summary>
    /// نمط خالد: Validator مع رسائل خطأ واضحة باللغة العربية
    /// </summary>
    public class ResendEmailConfirmationCommandValidator : BaseValidator<ResendEmailConfirmationCommand>
    {
        public ResendEmailConfirmationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح")
                .MaximumLength(256).WithMessage("البريد الإلكتروني طويل جداً");
        }
    }
}
