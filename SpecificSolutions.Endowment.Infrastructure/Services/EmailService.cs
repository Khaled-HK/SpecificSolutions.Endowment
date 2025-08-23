using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Global;
using System.Net;
using System.Net.Mail;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال البريد الإلكتروني
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// إرسال بريد إلكتروني
        /// </summary>
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
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
                        // Fallback to console logging for development
                        _logger.LogInformation("Email content for {Email}: Subject: {Subject}, Body: {Body}", to, subject, body);
                        await Task.Delay(100); // Simulate email sending
                        return true;
                    }
                },
                _logger,
                to);
        }

        /// <summary>
        /// إرسال البريد عبر SMTP
        /// </summary>
        private async Task<bool> SendEmailViaSmtpAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword)
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
                
                _logger.LogInformation("Email sent successfully to {Email}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP email sending failed to {Email}", to);
                return false;
            }
        }

        /// <summary>
        /// إرسال بريد تأكيد البريد الإلكتروني
        /// </summary>
        public async Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink)
        {
            _logger.LogInformation("Sending email confirmation to {Email}", email);
            
            var subject = "تأكيد البريد الإلكتروني - نظام الأوقاف";
            var userName = email.Split('@')[0]; // Extract username from email
            var body = EmailTemplates.GetEmailConfirmationTemplate(userName, confirmationLink);

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
            var body = EmailTemplates.GetPasswordResetTemplate(userName, resetLink);

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
            try
            {
                // التحقق من تنسيق البريد الإلكتروني
                var mailAddress = new MailAddress(email);

                // التحقق من أن البريد الإلكتروني يطابق التنسيق
                if (mailAddress.Address != email)
                {
                    return false;
                }

                // TODO: يمكن إضافة تحقق إضافي من وجود البريد الإلكتروني
                // مثل استخدام خدمة Email Validation API

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
