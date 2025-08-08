using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.ConfirmEmail;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class ConfirmEmailCommandValidator : BaseValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Messages.EmailRequired)
                .EmailAddress().WithMessage(Messages.EmailInvalid)
                .MaximumLength(100).WithMessage(Messages.EmailMaxLength);

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required");
        }
    }
} 