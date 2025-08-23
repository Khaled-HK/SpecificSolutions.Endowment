namespace SpecificSolutions.Endowment.Core.Entities
{
    /// <summary>
    /// تتبع البريد الإلكتروني
    /// </summary>
    public class EmailTracking
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public EmailType Type { get; set; }
        public EmailStatus Status { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public int RetryCount { get; set; } = 0;
        public string? ErrorMessage { get; set; }
        public string? TrackingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// حالة البريد الإلكتروني
    /// </summary>
    public enum EmailStatus
    {
        Pending,
        Sent,
        Delivered,
        Opened,
        Failed,
        Bounced
    }

    /// <summary>
    /// أنواع البريد الإلكتروني
    /// </summary>
    public enum EmailType
    {
        General,
        Confirmation,
        PasswordReset,
        Notification,
        Newsletter
    }
}
