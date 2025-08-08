using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ForgotPassword;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class ForgotPasswordCommandValidator : BaseValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Messages.EmailRequired)
                .EmailAddress().WithMessage(Messages.EmailInvalid)
                .MaximumLength(100).WithMessage(Messages.EmailMaxLength);
        }
    }
} 