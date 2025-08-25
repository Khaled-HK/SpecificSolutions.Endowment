using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Net.Http.Json;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال رموز التحقق عبر Discord Bot
    /// </summary>
    public class DiscordService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DiscordService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _webhookUrl;
        private readonly string _botToken;

        public DiscordService(
            HttpClient httpClient,
            ILogger<DiscordService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _webhookUrl = _configuration["Discord:WebhookUrl"] ?? string.Empty;
            _botToken = _configuration["Discord:BotToken"] ?? string.Empty;
        }

        /// <summary>
        /// إرسال رمز تحقق عبر Discord
        /// </summary>
        public async Task<bool> SendVerificationCodeViaDiscordAsync(string channelId, string code, string purpose)
        {
            try
            {
                if (string.IsNullOrEmpty(_botToken))
                {
                    _logger.LogWarning("Discord bot token is not configured");
                    return false;
                }

                var message = GenerateDiscordMessage(code, purpose);
                var url = $"https://discord.com/api/v10/channels/{channelId}/messages";

                var data = new
                {
                    content = message,
                    embeds = new[]
                    {
                        new
                        {
                            title = "🔐 رمز التحقق - نظام الأوقاف",
                            description = $"رمز التحقق الخاص بك هو: **{code}**\nصالح لمدة 15 دقيقة فقط",
                            color = 0x007bff,
                            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                            footer = new
                            {
                                text = "نظام الأوقاف"
                            }
                        }
                    }
                };

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", _botToken);
                _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                var response = await _httpClient.PostAsJsonAsync(url, data);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Discord verification code sent successfully to channel {ChannelId} for purpose {Purpose}", channelId, purpose);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send Discord message to {ChannelId}. Status: {Status}, Error: {Error}", 
                        channelId, response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending Discord verification code to {ChannelId} for purpose {Purpose}", channelId, purpose);
                return false;
            }
        }

        /// <summary>
        /// إنشاء رسالة Discord
        /// </summary>
        private string GenerateDiscordMessage(string code, string purpose)
        {
            var purposeText = purpose switch
            {
                "EmailConfirmation" => "تأكيد البريد الإلكتروني",
                "PasswordReset" => "إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "المصادقة الثنائية",
                _ => "التحقق"
            };

            return $"🔐 **رمز التحقق - نظام الأوقاف**\n\n📱 {purposeText}\n\n🔢 **رمز التحقق الخاص بك هو:**\n`{code}`\n\n⏰ **صالح لمدة 15 دقيقة فقط**\n\n⚠️ إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.\n\nشكراً لك،\nفريق نظام الأوقاف";
        }

        // Implementation of other interface methods (for compatibility)
        public async Task<bool> SendVerificationCodeViaEmailAsync(string email, string code, string purpose)
        {
            _logger.LogWarning("Email sending not supported by DiscordService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("SMS sending not supported by DiscordService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose)
        {
            _logger.LogWarning("Telegram sending not supported by DiscordService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("WhatsApp sending not supported by DiscordService");
            return false;
        }
    }
}
