namespace SpecificSolutions.Endowment.Application.Models.DTOs
{
    public class SendVerificationCodeRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty; // "EmailConfirmation", "PasswordReset", etc.
    }

    public class VerifyCodeRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class ResendVerificationCodeRequest
    {
        public string Email { get; set; } = string.Empty;
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
        public string PhoneNumber { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class SendTelegramVerificationCodeRequest
    {
        public string ChatId { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifySmsCodeRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifyTelegramCodeRequest
    {
        public string ChatId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    // DTOs للـ Push Notifications
    public class SendPushNotificationRequest
    {
        public string Subscription { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }

    public class VerifyPushNotificationRequest
    {
        public string Subscription { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}
