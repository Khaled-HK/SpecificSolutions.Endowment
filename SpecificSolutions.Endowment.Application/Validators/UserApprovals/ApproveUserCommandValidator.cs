using FluentValidation;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser;

namespace SpecificSolutions.Endowment.Application.Validators.UserApprovals
{
    /// <summary>
    /// Validator لـ ApproveUserCommand - نمط خالد
    /// التحقق من صحة البيانات فقط، وليس من قاعدة البيانات
    /// </summary>
    public class ApproveUserCommandValidator : BaseValidator<ApproveUserCommand>
    {
        public ApproveUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("معرف المستخدم مطلوب")
                .Must(BeValidGuid)
                .WithMessage("معرف المستخدم غير صحيح");

            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("اسم الدور مطلوب")
                .Must(BeValidRoleFormat)
                .WithMessage("اسم الدور يجب أن يكون نصاً صحيحاً");
        }

        private static bool BeValidGuid(string userId)
        {
            return Guid.TryParse(userId, out _);
        }

        private static bool BeValidRoleFormat(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return false;

            // التحقق من صحة تنسيق اسم الدور فقط
            // لا نتحقق من وجوده في قاعدة البيانات هنا
            return roleName.Length >= 2 && roleName.Length <= 50 && 
                   roleName.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-');
        }
    }
}
