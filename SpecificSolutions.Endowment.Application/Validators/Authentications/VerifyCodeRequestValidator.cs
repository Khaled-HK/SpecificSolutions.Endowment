using FluentValidation;
using SpecificSolutions.Endowment.Application.Models.DTOs;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class VerifyCodeRequestValidator : BaseValidator<VerifyCodeRequest>
    {
        public VerifyCodeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress()
                .WithMessage("البريد الإلكتروني غير صحيح")
                .MaximumLength(256)
                .WithMessage("البريد الإلكتروني يجب أن لا يتجاوز 256 حرف");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("رمز التحقق مطلوب")
                .Length(6, 6)
                .WithMessage("رمز التحقق يجب أن يكون 6 أرقام")
                .Matches(@"^\d{6}$")
                .WithMessage("رمز التحقق يجب أن يحتوي على أرقام فقط");

            RuleFor(x => x.Purpose)
                .NotEmpty()
                .WithMessage("الغرض مطلوب")
                .MaximumLength(50)
                .WithMessage("الغرض يجب أن لا يتجاوز 50 حرف")
                .Must(purpose => new[] { "EmailConfirmation", "PasswordReset", "TwoFactorAuth" }.Contains(purpose))
                .WithMessage("الغرض يجب أن يكون أحد القيم التالية: EmailConfirmation, PasswordReset, TwoFactorAuth");
        }
    }
}
