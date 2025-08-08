using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ResetPassword;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class ResetPasswordCommandValidator : BaseValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Messages.EmailRequired)
                .EmailAddress().WithMessage(Messages.EmailInvalid)
                .MaximumLength(100).WithMessage(Messages.EmailMaxLength);

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(Messages.PasswordRequired)
                .MinimumLength(6).WithMessage(Messages.PasswordMinLength)
                .MaximumLength(100).WithMessage(Messages.PasswordMaxLength);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(Messages.ConfirmPasswordRequired)
                .Equal(x => x.NewPassword).WithMessage(Messages.PasswordsDoNotMatch);
        }
    }
} 