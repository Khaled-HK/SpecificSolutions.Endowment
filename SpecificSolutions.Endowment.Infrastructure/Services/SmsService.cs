using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Net.Http.Json;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال رموز التحقق عبر SMS
    /// </summary>
    public class SmsService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SmsService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _twilioAccountSid;
        private readonly string _twilioAuthToken;
        private readonly string _twilioPhoneNumber;

        public SmsService(
            HttpClient httpClient,
            ILogger<SmsService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _twilioAccountSid = _configuration["Sms:TwilioAccountSid"] ?? string.Empty;
            _twilioAuthToken = _configuration["Sms:TwilioAuthToken"] ?? string.Empty;
            _twilioPhoneNumber = _configuration["Sms:TwilioPhoneNumber"] ?? string.Empty;
        }

        /// <summary>
        /// إرسال رمز تحقق عبر SMS
        /// </summary>
        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string code, string purpose)
        {
            try
            {
                if (string.IsNullOrEmpty(_twilioAccountSid) || string.IsNullOrEmpty(_twilioAuthToken))
                {
                    _logger.LogWarning("Twilio credentials are not configured");
                    return false;
                }

                var message = GenerateSmsMessage(code, purpose);
                var url = $"https://api.twilio.com/2010-04-01/Accounts/{_twilioAccountSid}/Messages.json";

                // إعداد Basic Authentication
                var authToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_twilioAccountSid}:{_twilioAuthToken}"));
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

                var formData = new List<KeyValuePair<string, string>>
                {
                    new("To", phoneNumber),
                    new("From", _twilioPhoneNumber),
                    new("Body", message)
                };

                var content = new FormUrlEncodedContent(formData);
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("SMS verification code sent successfully to {PhoneNumber} for purpose {Purpose}", phoneNumber, purpose);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send SMS to {PhoneNumber}. Status: {Status}, Error: {Error}", 
                        phoneNumber, response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS verification code to {PhoneNumber} for purpose {Purpose}", phoneNumber, purpose);
                return false;
            }
        }

        /// <summary>
        /// إنشاء رسالة SMS
        /// </summary>
        private string GenerateSmsMessage(string code, string purpose)
        {
            var purposeText = purpose switch
            {
                "EmailConfirmation" => "تأكيد البريد الإلكتروني",
                "PasswordReset" => "إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "المصادقة الثنائية",
                _ => "التحقق"
            };

            return $"رمز التحقق - نظام الأوقاف\n{purposeText}\nالرمز: {code}\nصالح لمدة 15 دقيقة";
        }

        // Implementation of other interface methods (for compatibility)
        public async Task<bool> SendVerificationCodeViaEmailAsync(string email, string code, string purpose)
        {
            _logger.LogWarning("Email sending not implemented in SmsService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose)
        {
            _logger.LogWarning("Telegram sending not implemented in SmsService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("WhatsApp sending not implemented in SmsService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaPushNotificationAsync(string subscription, string code, string purpose)
        {
            _logger.LogWarning("Push notification sending not implemented in SmsService");
            return false;
        }
        {
            _logger.LogWarning("Email sending not supported by SmsService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose)
        {
            _logger.LogWarning("Telegram sending not supported by SmsService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("WhatsApp sending not supported by SmsService");
            return false;
        }
    }
}
