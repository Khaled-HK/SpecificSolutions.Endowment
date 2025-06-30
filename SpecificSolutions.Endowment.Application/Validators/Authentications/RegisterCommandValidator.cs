using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");

            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password).MinimumLength(10).WithMessage(Messages.PasswordMustMore6Characters);

            RuleFor(x => x.Password).Matches(@"[A-Z]+")
                .WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.Password).Matches(@"[a-z]+")
                .WithMessage("Password must contain at least one lowercase letter.");

            RuleFor(x => x.Password).Matches(@"[0-9]+")
                .WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Password).Matches(@"[!@#$%^&*]+")
                .WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Password).Matches(@"[a-zA-Z0-9!@#$%^&*]+")
                .WithMessage("Password must contain at least one letter, one number and one special character.");

            RuleFor(x => x.Password).Matches(@"^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^a-zA-Z\d])\S{6,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");

            RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])\S{6,}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.FirstName).MinimumLength(2).WithMessage("First name must be at least 2 characters.");
            RuleFor(x => x.LastName).MinimumLength(2).WithMessage("Last name must be at least 2 characters.");

            RuleFor(x => x.FirstName).MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.PhoneNumber).Matches(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")
                .WithMessage("Invalid phone number.");

            RuleFor(x => x.PhoneNumber).MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.");

            RuleFor(x => x.Address).MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");

            RuleFor(x => x.City).MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

            RuleFor(x => x.Country).MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");

        }

        // for login command
        // public class LoginCommandValidator : AbstractValidator<LoginCommand>
        // {
        //     public LoginCommandValidator()
        //     {
        //         RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        //         RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        //
        //         RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email address.");
        //
        //         RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        //
        //         RuleFor(x => x.Password).Matches(@"[A-Z]+")
        //             .WithMessage("Password must contain at least one uppercase letter.");
        //
        //         RuleFor(x => x.Password).Matches(@"[a-z]+")
        //             .WithMessage("Password must contain at least one lowercase letter.");
        //
        //         RuleFor(x => x.Password).Matches(@"[0-9]+")
        //             .WithMessage("Password must contain at least one number.");
        //
        //         RuleFor(x => x.Password).Matches(@"[!@#$%^&*]+")
        //             .WithMessage("Password must contain at least one special character.");
        //
        //         RuleFor(x => x.Password).Matches(@"[a-zA-Z0-9!@#$%^&*]+")
        //             .WithMessage("Password must contain at least one letter, one number and one special character.");
        //
        //         RuleFor(x => x.Password).Matches(@"^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^a-zA-Z\d])\S{6,}$")
        //             .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
        //
        //         RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])\S{6,}$")
        //             .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
        //      
        //      
        //      
    }
}