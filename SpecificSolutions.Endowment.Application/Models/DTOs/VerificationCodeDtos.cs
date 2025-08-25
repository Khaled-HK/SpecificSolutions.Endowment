using System.ComponentModel.DataAnnotations;

namespace SpecificSolutions.Endowment.Application.Models.DTOs
{
    public class SendVerificationCodeRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty; // "EmailConfirmation", "PasswordReset", etc.
    }

    public class VerifyCodeRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "رمز التحقق يجب أن يكون 6 أرقام")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "رمز التحقق يجب أن يحتوي على أرقام فقط")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }

    public class ResendVerificationCodeRequest
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerificationCodeResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? RemainingSeconds { get; set; }
        public string? Email { get; set; }
        public string? Purpose { get; set; }
    }

    public class VerificationCodeInfo
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public bool IsExpired { get; set; }
        public int RemainingSeconds { get; set; }
    }

    // DTOs للآليات الجديدة
    public class SendSmsVerificationCodeRequest
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }

    public class SendTelegramVerificationCodeRequest
    {
        [Required(ErrorMessage = "معرف المحادثة مطلوب")]
        public string ChatId { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifySmsCodeRequest
    {
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "رمز التحقق يجب أن يكون 6 أرقام")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "رمز التحقق يجب أن يحتوي على أرقام فقط")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifyTelegramCodeRequest
    {
        [Required(ErrorMessage = "معرف المحادثة مطلوب")]
        public string ChatId { get; set; } = string.Empty;

        [Required(ErrorMessage = "رمز التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "رمز التحقق يجب أن يكون 6 أرقام")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "رمز التحقق يجب أن يحتوي على أرقام فقط")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "الغرض مطلوب")]
        public string Purpose { get; set; } = string.Empty;
    }
}
