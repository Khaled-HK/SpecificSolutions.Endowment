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
                UseSmtp = false // Use fallback for testing
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
    }
}
