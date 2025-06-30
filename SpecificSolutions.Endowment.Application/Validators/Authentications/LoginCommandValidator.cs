using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

            //RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email address.");

            //RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            //RuleFor(x => x.Password).Matches(@"[A-Z]+")
            //    .WithMessage("Password must contain at least one uppercase letter.");

            //RuleFor(x => x.Password).Matches(@"[a-z]+")
            //    .WithMessage("Password must contain at least one lowercase letter.");

            //RuleFor(x => x.Password).Matches(@"[0-9]+")
            //    .WithMessage("Password must contain at least one number.");

            //RuleFor(x => x.Password).Matches(@"[!@#$%^&*]+")
            //    .WithMessage("Password must contain at least one special character.");

            //RuleFor(x => x.Password).Matches(@"[a-zA-Z0-9!@#$%^&*]+")
            //    .WithMessage("Password must contain at least one letter, one number and one special character.");

            //RuleFor(x => x.Password).Matches(@"^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^a-zA-Z\d])\S{6,}$")
            //    .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");

            //RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])\S{6,}$")
            //    .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
        }
    }
}
