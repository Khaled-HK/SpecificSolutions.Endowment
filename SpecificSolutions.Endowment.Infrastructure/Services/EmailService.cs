using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Global;
using System.Net;
using System.Net.Mail;
using System.Collections.Concurrent;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال البريد الإلكتروني - النسخة المجانية المحسنة
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;
        private readonly ConcurrentDictionary<string, DateTime> _emailRateLimit;
        private readonly ConcurrentDictionary<string, int> _dailyEmailCount;
        private readonly DateTime _lastResetDate;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
            _emailRateLimit = new ConcurrentDictionary<string, DateTime>();
            _dailyEmailCount = new ConcurrentDictionary<string, int>();
            _lastResetDate = DateTime.Today;
        }

        /// <summary>
        /// إرسال بريد إلكتروني مع التحقق من القيود المجانية
        /// </summary>
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            // التحقق من صحة البريد الإلكتروني
            if (_emailSettings.EnableEmailValidation && !await ValidateEmailAsync(to))
            {
                _logger.LogWarning("Invalid email format: {Email}", to);
                return false;
            }

            // التحقق من Rate Limiting
            if (!CheckRateLimit(to))
            {
                _logger.LogWarning("Rate limit exceeded for email: {Email}", to);
                return false;
            }

            // التحقق من الحد اليومي
            if (!CheckDailyLimit())
            {
                _logger.LogWarning("Daily email limit exceeded. Limit: {Limit}", _emailSettings.MaxEmailsPerDay);
                return false;
            }

            return await EmailRetryPolicy.ExecuteWithRetryAsync(
                async () =>
                {
                    _logger.LogInformation("Attempting to send email to {Email} with subject: {Subject}", to, subject);

                    if (_emailSettings.UseSmtp)
                    {
                        return await SendEmailViaSmtpAsync(to, subject, body);
                    }
                    else
                    {
                        // Fallback to console logging for development (مجاني)
                        return await SendEmailViaLoggingAsync(to, subject, body);
                    }
                },
                _logger,
                to);
        }

        /// <summary>
        /// إرسال البريد عبر SMTP (Gmail المجاني)
        /// </summary>
        private async Task<bool> SendEmailViaSmtpAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                    Timeout = 10000 // 10 seconds timeout
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                
                // تحديث العداد اليومي
                IncrementDailyCount();
                
                _logger.LogInformation("Email sent successfully to {Email} via SMTP", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP email sending failed to {Email}", to);
                return false;
            }
        }

        /// <summary>
        /// إرسال البريد عبر Logging (مجاني للتطوير)
        /// </summary>
        private async Task<bool> SendEmailViaLoggingAsync(string to, string subject, string body)
        {
            try
            {
                // محاكاة إرسال البريد
                await Task.Delay(100);

                // طباعة البريد في Logs (مجاني)
                _logger.LogInformation("=== EMAIL CONTENT (Development Mode) ===");
                _logger.LogInformation("To: {Email}", to);
                _logger.LogInformation("Subject: {Subject}", subject);
                _logger.LogInformation("Body: {Body}", body);
                _logger.LogInformation("=== END EMAIL CONTENT ===");

                // تحديث العداد اليومي
                IncrementDailyCount();

                _logger.LogInformation("Email logged successfully to {Email} (Development Mode)", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email logging failed to {Email}", to);
                return false;
            }
        }

        /// <summary>
        /// التحقق من Rate Limiting
        /// </summary>
        private bool CheckRateLimit(string email)
        {
            var now = DateTime.UtcNow;
            var lastEmailTime = _emailRateLimit.GetOrAdd(email, now);

            if ((now - lastEmailTime).TotalMinutes < 1.0 / _emailSettings.EmailRateLimit)
            {
                return false;
            }

            _emailRateLimit[email] = now;
            return true;
        }

        /// <summary>
        /// التحقق من الحد اليومي
        /// </summary>
        private bool CheckDailyLimit()
        {
            var today = DateTime.Today;
            if (today > _lastResetDate)
            {
                // إعادة تعيين العداد اليومي
                _dailyEmailCount.Clear();
            }

            var currentCount = _dailyEmailCount.GetOrAdd("total", 0);
            return currentCount < _emailSettings.MaxEmailsPerDay;
        }

        /// <summary>
        /// زيادة العداد اليومي
        /// </summary>
        private void IncrementDailyCount()
        {
            _dailyEmailCount.AddOrUpdate("total", 1, (key, oldValue) => oldValue + 1);
        }

        /// <summary>
        /// إرسال بريد تأكيد البريد الإلكتروني
        /// </summary>
        public async Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            _logger.LogInformation("Sending email confirmation to {Email}", email);
            
            var subject = "تأكيد البريد الإلكتروني - نظام الأوقاف";
            var userName = email.Split('@')[0]; // Extract username from email
            
            string body;
            if (_emailSettings.EnableEmailTemplates)
            {
                body = EmailTemplates.GetEmailConfirmationTemplate(userName, confirmationLink);
            }
            else
            {
                body = $"مرحباً {userName}، يرجى النقر على الرابط التالي لتأكيد بريدك الإلكتروني: {confirmationLink}";
            }

            var result = await SendEmailAsync(email, subject, body);
            
            if (result)
                _logger.LogInformation("Email confirmation sent successfully to {Email}", email);
            else
                _logger.LogError("Failed to send email confirmation to {Email}", email);
                
            return result;
        }

        /// <summary>
        /// إرسال بريد إعادة تعيين كلمة المرور
        /// </summary>
        public async Task<bool> SendPasswordResetAsync(string email, string resetLink)
        {
            _logger.LogInformation("Sending password reset email to {Email}", email);
            
            var subject = "إعادة تعيين كلمة المرور - نظام الأوقاف";
            var userName = email.Split('@')[0]; // Extract username from email
            
            string body;
            if (_emailSettings.EnableEmailTemplates)
            {
                body = EmailTemplates.GetPasswordResetTemplate(userName, resetLink);
            }
            else
            {
                body = $"مرحباً {userName}، يرجى النقر على الرابط التالي لإعادة تعيين كلمة المرور: {resetLink}";
            }

            var result = await SendEmailAsync(email, subject, body);
            
            if (result)
                _logger.LogInformation("Password reset email sent successfully to {Email}", email);
            else
                _logger.LogError("Failed to send password reset email to {Email}", email);
                
            return result;
        }

        /// <summary>
        /// التحقق من صحة البريد الإلكتروني
        /// </summary>
        public async Task<bool> ValidateEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // التحقق من تنسيق البريد الإلكتروني
                var mailAddress = new MailAddress(email);

                // التحقق من أن البريد الإلكتروني يطابق التنسيق
                if (mailAddress.Address != email)
                {
                    return false;
                }

                // التحقق من طول البريد الإلكتروني
                if (email.Length > 254)
                {
                    return false;
                }

                // التحقق من وجود @ و . في البريد الإلكتروني
                if (!email.Contains('@') || !email.Contains('.'))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// الحصول على إحصائيات البريد الإلكتروني
        /// </summary>
        public Dictionary<string, object> GetEmailStatistics()
        {
            return new Dictionary<string, object>
            {
                ["DailyEmailCount"] = _dailyEmailCount.GetOrAdd("total", 0),
                ["MaxEmailsPerDay"] = _emailSettings.MaxEmailsPerDay,
                ["EmailRateLimit"] = _emailSettings.EmailRateLimit,
                ["UseSmtp"] = _emailSettings.UseSmtp,
                ["EnableEmailValidation"] = _emailSettings.EnableEmailValidation,
                ["EnableEmailTracking"] = _emailSettings.EnableEmailTracking
            };
        }
    }
}
