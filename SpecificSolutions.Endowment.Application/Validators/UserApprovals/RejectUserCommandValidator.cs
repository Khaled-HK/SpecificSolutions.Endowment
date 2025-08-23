using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.RejectUser;

namespace SpecificSolutions.Endowment.Application.Validators.UserApprovals
{
    /// <summary>
    /// Validator لـ RejectUserCommand - نمط خالد
    /// </summary>
    public class RejectUserCommandValidator : BaseValidator<RejectUserCommand>
    {
        public RejectUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("معرف المستخدم مطلوب")
                .Must(BeValidGuid)
                .WithMessage("معرف المستخدم غير صحيح");
        }


    }
}
