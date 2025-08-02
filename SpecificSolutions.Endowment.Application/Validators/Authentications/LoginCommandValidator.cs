using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Validators.Authentications
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Messages.EmailRequired);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Messages.PasswordRequired);

            //RuleFor(x => x.Email).EmailAddress().WithMessage(Messages.EmailInvalid);

            //RuleFor(x => x.Password).MinimumLength(6).WithMessage(Messages.PasswordMinLength);

            //RuleFor(x => x.Password).Matches(@"[A-Z]+")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"[a-z]+")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على حرف صغير واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"[0-9]+")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على رقم واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"[!@#$%^&*]+")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على رمز خاص واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"[a-zA-Z0-9!@#$%^&*]+")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على حرف واحد ورقم واحد ورمز خاص واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"^(?=\S*[a-z])(?=\S*[A-Z])(?=\S*\d)(?=\S*[^a-zA-Z\d])\S{6,}$")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير واحد وحرف صغير واحد ورقم واحد ورمز خاص واحد على الأقل.");

            //RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])\S{6,}$")
            //    .WithMessage("كلمة المرور يجب أن تحتوي على حرف كبير واحد وحرف صغير واحد ورقم واحد ورمز خاص واحد على الأقل.");
        }
    }
}
