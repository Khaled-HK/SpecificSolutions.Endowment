using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ChangePassword;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class ChangePasswordCommandValidator : BaseValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

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