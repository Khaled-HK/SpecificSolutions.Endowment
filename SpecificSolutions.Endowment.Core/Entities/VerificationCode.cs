namespace SpecificSolutions.Endowment.Core.Entities
{
    public class VerificationCode
    {
        public int Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; } = false;
        public bool IsExpired { get; private set; } = false;
        public string? Purpose { get; private set; } // "EmailConfirmation", "PasswordReset", etc.
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }
        public string? UserId { get; private set; }

        // Private constructor for EF Core
        private VerificationCode() { }

        // Factory method for creating a new VerificationCode
        public static VerificationCode Create(string email, string code, DateTime expiresAt, string? purpose = null, string? ipAddress = null, string? userAgent = null, string? userId = null)
        {
            return new VerificationCode
            {
                Email = email,
                Code = code,
                ExpiresAt = expiresAt,
                Purpose = purpose,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                UserId = userId
            };
        }

        // Helper Methods
        public bool IsValid()
        {
            return !IsUsed && !IsExpired && DateTime.UtcNow < ExpiresAt;
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public void MarkAsExpired()
        {
            IsExpired = true;
        }

        public TimeSpan GetRemainingTime()
        {
            if (IsExpired || IsUsed)
                return TimeSpan.Zero;

            var remaining = ExpiresAt - DateTime.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }
}
