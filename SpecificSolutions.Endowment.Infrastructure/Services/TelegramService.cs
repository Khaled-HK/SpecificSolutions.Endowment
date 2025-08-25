using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Net.Http.Json;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال رموز التحقق عبر Telegram Bot
    /// </summary>
    public class TelegramService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TelegramService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _botToken;
        private readonly string _botUsername;

        public TelegramService(
            HttpClient httpClient,
            ILogger<TelegramService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _botToken = _configuration["Telegram:BotToken"] ?? string.Empty;
            _botUsername = _configuration["Telegram:BotUsername"] ?? string.Empty;
        }

        /// <summary>
        /// إرسال رمز تحقق عبر Telegram
        /// </summary>
        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose)
        {
            try
            {
                if (string.IsNullOrEmpty(_botToken))
                {
                    _logger.LogWarning("Telegram bot token is not configured");
                    return false;
                }

                var message = GenerateTelegramMessage(code, purpose);
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

                var data = new
                {
                    chat_id = chatId,
                    text = message,
                    parse_mode = "HTML",
                    disable_web_page_preview = true
                };

                var response = await _httpClient.PostAsJsonAsync(url, data);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Telegram verification code sent successfully to chat {ChatId} for purpose {Purpose}", chatId, purpose);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send Telegram message to {ChatId}. Status: {Status}, Error: {Error}", 
                        chatId, response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Telegram verification code to {ChatId} for purpose {Purpose}", chatId, purpose);
                return false;
            }
        }

        /// <summary>
        /// إنشاء رسالة Telegram
        /// </summary>
        private string GenerateTelegramMessage(string code, string purpose)
        {
            var purposeText = purpose switch
            {
                "EmailConfirmation" => "تأكيد البريد الإلكتروني",
                "PasswordReset" => "إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "المصادقة الثنائية",
                _ => "التحقق"
            };

            return $@"🔐 <b>رمز التحقق - نظام الأوقاف</b>

📱 {purposeText}

🔢 <b>رمز التحقق الخاص بك هو:</b>
<code>{code}</code>

⏰ <b>صالح لمدة 15 دقيقة فقط</b>

⚠️ إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.

شكراً لك،
فريق نظام الأوقاف";
        }

        // Implementation of other interface methods (for compatibility)
        public async Task<bool> SendVerificationCodeViaEmailAsync(string email, string code, string purpose)
        {
            _logger.LogWarning("Email sending not supported by TelegramService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("SMS sending not supported by TelegramService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("WhatsApp sending not supported by TelegramService");
            return false;
        }
    }
}
