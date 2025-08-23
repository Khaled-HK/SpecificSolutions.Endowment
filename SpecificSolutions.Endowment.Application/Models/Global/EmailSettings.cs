namespace SpecificSolutions.Endowment.Application.Models.Global
{
    /// <summary>
    /// إعدادات البريد الإلكتروني - النسخة المجانية المحسنة
    /// </summary>
    public class EmailSettings
    {
        // إعدادات SMTP الأساسية
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = true;
        public string ApiKey { get; set; } = string.Empty;
        public string FromAddress { get; set; } = string.Empty;
        public bool UseSmtp { get; set; } = false;

        // إعدادات مجانية محسنة
        public int MaxEmailsPerDay { get; set; } = 500; // حد Gmail المجاني
        public int EmailRateLimit { get; set; } = 10; // بريد في الدقيقة
        public bool EnableEmailValidation { get; set; } = true;
        public bool EnableEmailTracking { get; set; } = true;
        public int RetryAttempts { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;

        // إعدادات إضافية مجانية
        public bool EnableFallbackLogging { get; set; } = true;
        public bool EnableEmailQueue { get; set; } = true;
        public int QueueProcessingIntervalSeconds { get; set; } = 30;
        public bool EnableEmailTemplates { get; set; } = true;
        public string DefaultLanguage { get; set; } = "ar";
    }
}
