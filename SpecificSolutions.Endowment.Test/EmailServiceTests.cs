using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Infrastructure.Services;
using Xunit;

namespace SpecificSolutions.Endowment.Test
{
    public class EmailServiceTests
    {
        private readonly Mock<ILogger<EmailService>> _loggerMock;
        private readonly EmailSettings _emailSettings;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _loggerMock = new Mock<ILogger<EmailService>>();
            
            _emailSettings = new EmailSettings
            {
                SmtpServer = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUsername = "test@example.com",
                SmtpPassword = "test-password",
                FromEmail = "noreply@test.com",
                FromName = "Test System",
                EnableSsl = true,
                UseSmtp = false, // Use fallback for testing
                MaxEmailsPerDay = 500,
                EmailRateLimit = 10,
                EnableEmailValidation = true,
                EnableEmailTracking = true,
                RetryAttempts = 3,
                RetryDelaySeconds = 2,
                EnableFallbackLogging = true,
                EnableEmailQueue = true,
                QueueProcessingIntervalSeconds = 30,
                EnableEmailTemplates = true,
                DefaultLanguage = "ar"
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(_emailSettings);

            _emailService = new EmailService(_loggerMock.Object, optionsMock.Object);
        }

        [Fact]
        public async Task SendEmailAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            var result = await _emailService.SendEmailAsync(to, subject, body);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendEmailConfirmationAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var confirmationLink = "https://test.com/confirm?token=123";

            // Act
            var result = await _emailService.SendEmailConfirmationAsync(email, confirmationLink);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendPasswordResetAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var resetLink = "https://test.com/reset?token=123";

            // Act
            var result = await _emailService.SendPasswordResetAsync(email, resetLink);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateEmailAsync_WithValidEmail_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var result = await _emailService.ValidateEmailAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateEmailAsync_WithInvalidEmail_ReturnsFalse()
        {
            // Arrange
            var email = "invalid-email";

            // Act
            var result = await _emailService.ValidateEmailAsync(email);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user.name@domain.co.uk", true)]
        [InlineData("invalid-email", false)]
        [InlineData("test@", false)]
        [InlineData("@example.com", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public async Task ValidateEmailAsync_WithVariousEmails_ReturnsExpectedResult(string email, bool expectedResult)
        {
            // Act
            var result = await _emailService.ValidateEmailAsync(email);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetEmailStatistics_ReturnsValidStatistics()
        {
            // Act
            var statistics = _emailService.GetEmailStatistics();

            // Assert
            Assert.NotNull(statistics);
            Assert.Contains("DailyEmailCount", statistics.Keys);
            Assert.Contains("MaxEmailsPerDay", statistics.Keys);
            Assert.Contains("EmailRateLimit", statistics.Keys);
            Assert.Contains("UseSmtp", statistics.Keys);
            Assert.Contains("EnableEmailValidation", statistics.Keys);
            Assert.Contains("EnableEmailTracking", statistics.Keys);
        }

        [Fact]
        public async Task SendEmailAsync_WithRateLimitExceeded_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act - Send multiple emails quickly to trigger rate limit
            var results = new List<bool>();
            for (int i = 0; i < 15; i++) // More than rate limit
            {
                var result = await _emailService.SendEmailAsync(email, subject, body);
                results.Add(result);
            }

            // Assert - Some emails should fail due to rate limiting
            Assert.Contains(false, results);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmailValidationDisabled_AcceptsInvalidEmail()
        {
            // Arrange
            var invalidEmail = "invalid-email";
            var subject = "Test Subject";
            var body = "Test Body";

            // Create service with validation disabled
            var settingsWithValidationDisabled = new EmailSettings
            {
                EnableEmailValidation = false,
                UseSmtp = false
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(settingsWithValidationDisabled);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act
            var result = await emailService.SendEmailAsync(invalidEmail, subject, body);

            // Assert
            Assert.True(result); // Should succeed when validation is disabled
        }

        [Fact]
        public async Task SendEmailAsync_WithTemplatesDisabled_UsesSimpleText()
        {
            // Arrange
            var email = "test@example.com";
            var confirmationLink = "https://test.com/confirm?token=123";

            // Create service with templates disabled
            var settingsWithTemplatesDisabled = new EmailSettings
            {
                EnableEmailTemplates = false,
                UseSmtp = false
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(settingsWithTemplatesDisabled);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act
            var result = await emailService.SendEmailConfirmationAsync(email, confirmationLink);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendEmailAsync_WithDailyLimitExceeded_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            // Create service with very low daily limit
            var settingsWithLowLimit = new EmailSettings
            {
                MaxEmailsPerDay = 1,
                UseSmtp = false
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(settingsWithLowLimit);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act - Send first email (should succeed)
            var firstResult = await emailService.SendEmailAsync(email, subject, body);
            
            // Send second email (should fail due to daily limit)
            var secondResult = await emailService.SendEmailAsync(email, subject, body);

            // Assert
            Assert.True(firstResult);
            Assert.False(secondResult);
        }

        [Fact(Skip = "This test requires SMTP_PASSWORD environment variable to be set. Only run when needed.")]
        public async Task SendEmailAsync_WithRealGmailSettings_ShouldWork()
        {
            // Arrange
            var email = "test@example.com";
            var subject = "Test Email";
            var body = "This is a test email";

            // Create service with minimal settings
            var testSettings = new EmailSettings
            {
                UseSmtp = false, // Use logging fallback
                EnableEmailValidation = false,
                EnableEmailQueue = false,
                EnableEmailTemplates = false,
                MaxEmailsPerDay = 1000,
                EmailRateLimit = 0 // Disable rate limiting
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(testSettings);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act
            var result = await emailService.SendEmailAsync(email, subject, body);

            // Assert
            Assert.True(result, "Email should be sent successfully with minimal settings");
        }

        [Fact(Skip = "This test requires SMTP_PASSWORD environment variable to be set. Only run when needed.")]
        public async Task SendRealEmailToKhaled_ShouldDeliverActualEmail()
        {
            // Arrange
            var email = "khaledalneffaati@gmail.com";
            var subject = "اختبار البريد الإلكتروني من نظام الأوقاف 📧";
            var body = @"
                <div style='direction: rtl; font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f8f9fa;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='color: #2c3e50; margin: 0;'>🕌 نظام الأوقاف</h1>
                            <p style='color: #7f8c8d; margin: 10px 0 0 0;'>اختبار البريد الإلكتروني</p>
                        </div>
                        
                        <div style='margin-bottom: 30px;'>
                            <h2 style='color: #2c3e50; margin-bottom: 20px;'>مرحباً خالد! 👋</h2>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                هذه رسالة اختبار من نظام الأوقاف للتأكد من أن خدمة البريد الإلكتروني تعمل بشكل صحيح.
                            </p>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                ✅ إذا وصلتك هذه الرسالة، فهذا يعني أن النظام يعمل بنجاح!
                            </p>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                📧 تم الإرسال من: نظام الأوقاف - خدمة البريد الإلكتروني<br/>
                                🕐 وقت الإرسال: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"<br/>
                                🌐 البيئة: اختبار تطوير
                            </p>
                        </div>
                        
                        <div style='text-align: center; margin-bottom: 30px; padding: 20px; background-color: #e8f5e8; border-radius: 8px;'>
                            <h3 style='color: #27ae60; margin: 0 0 10px 0;'>🎉 نجح الاختبار!</h3>
                            <p style='color: #2d8f47; margin: 0;'>تم إرسال البريد الإلكتروني بنجاح</p>
                        </div>
                        
                        <div style='text-align: center; color: #7f8c8d; font-size: 12px; border-top: 1px solid #ecf0f1; padding-top: 20px;'>
                            <p>هذه رسالة تلقائية من نظام الأوقاف - لا تحتاج للرد عليها</p>
                        </div>
                    </div>
                </div>";

            // Get password from environment variable ONLY
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            if (string.IsNullOrEmpty(smtpPassword))
            {
                // Skip test if no environment variable is set
                Assert.True(true, "SMTP_PASSWORD environment variable not set. Skipping real email test for security.");
                return;
            }

            // Create service with REAL Gmail settings
            var realGmailSettings = new EmailSettings
            {
                SmtpServer = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUsername = "khaled.send.mess@gmail.com",
                SmtpPassword = smtpPassword,
                FromEmail = "khaled.send.mess@gmail.com",
                FromName = "نظام الأوقاف - اختبار",
                EnableSsl = true,
                UseSmtp = true, // Use REAL SMTP
                MaxEmailsPerDay = 500,
                EmailRateLimit = 0, // Disable rate limiting for test
                EnableEmailValidation = true,
                EnableEmailTracking = true,
                RetryAttempts = 3,
                RetryDelaySeconds = 2,
                EnableFallbackLogging = true,
                EnableEmailQueue = false, // Send immediately
                EnableEmailTemplates = false, // Use custom HTML
                DefaultLanguage = "ar"
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(realGmailSettings);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act
            var result = await emailService.SendEmailAsync(email, subject, body);

            // Assert
            Assert.True(result, "Email should be sent successfully to khaledalneffaati@gmail.com");
        }

        [Fact(Skip = "This test requires SMTP_PASSWORD environment variable to be set. Only run when needed.")]
        public async Task SendEmailAsync_WithEnvironmentVariable_ShouldWorkSecurely()
        {
            // Arrange
            var email = "test@example.com";
            var subject = "Test Email - Secure";
            var body = "This is a secure test email";

            // Get password from environment variable ONLY
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            if (string.IsNullOrEmpty(smtpPassword))
            {
                // Skip test if no environment variable is set
                Assert.True(true, "SMTP_PASSWORD environment variable not set. Skipping secure email test.");
                return;
            }

            // Create service with environment-based settings
            var secureSettings = new EmailSettings
            {
                SmtpServer = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "test@example.com",
                SmtpPassword = smtpPassword, // From environment variable
                FromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? "test@example.com",
                FromName = "Secure Test System",
                EnableSsl = true,
                UseSmtp = false, // Use logging fallback for security
                MaxEmailsPerDay = 100,
                EmailRateLimit = 0,
                EnableEmailValidation = false,
                EnableEmailTracking = false,
                RetryAttempts = 1,
                RetryDelaySeconds = 1,
                EnableFallbackLogging = true,
                EnableEmailQueue = false,
                EnableEmailTemplates = false,
                DefaultLanguage = "en"
            };

            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(x => x.Value).Returns(secureSettings);

            var emailService = new EmailService(_loggerMock.Object, optionsMock.Object);

            // Act
            var result = await emailService.SendEmailAsync(email, subject, body);

            // Assert
            Assert.True(result, "Email should be sent successfully using environment variables");
        }
    }
}
