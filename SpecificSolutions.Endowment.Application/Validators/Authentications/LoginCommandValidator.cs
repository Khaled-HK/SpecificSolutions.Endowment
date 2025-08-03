using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class LoginCommandValidator : BaseValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(Messages.EmailRequired)
                .EmailAddress()
                .WithMessage(Messages.EmailInvalid)
                .MaximumLength(100)
                .WithMessage(Messages.EmailMaxLength);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(Messages.PasswordRequired)
                .MinimumLength(3)
                .WithMessage(Messages.PasswordMinLength3)
                .MaximumLength(100)
                .WithMessage(Messages.PasswordMaxLength);
        }
    }
}
