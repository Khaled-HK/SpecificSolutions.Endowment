using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// خدمة إرسال رموز التحقق عبر Push Notifications
    /// </summary>
    public class PushNotificationService : IMessagingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PushNotificationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _vapidPublicKey;
        private readonly string _vapidPrivateKey;
        private readonly string _firebaseServerKey;

        public PushNotificationService(
            HttpClient httpClient,
            ILogger<PushNotificationService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _vapidPublicKey = _configuration["PushNotifications:VapidPublicKey"] ?? string.Empty;
            _vapidPrivateKey = _configuration["PushNotifications:VapidPrivateKey"] ?? string.Empty;
            _firebaseServerKey = _configuration["PushNotifications:FirebaseServerKey"] ?? string.Empty;
        }

        /// <summary>
        /// إرسال رمز تحقق عبر Push Notification
        /// </summary>
        public async Task<bool> SendVerificationCodeViaPushNotificationAsync(string subscription, string code, string purpose)
        {
            try
            {
                if (string.IsNullOrEmpty(_vapidPublicKey) || string.IsNullOrEmpty(_vapidPrivateKey))
                {
                    _logger.LogWarning("Push notification VAPID keys are not configured");
                    return false;
                }

                var message = GeneratePushMessage(code, purpose);
                var payload = new
                {
                    title = "🔐 رمز التحقق - نظام الأوقاف",
                    body = message,
                    icon = "/favicon.ico",
                    badge = "/badge.png",
                    data = new
                    {
                        code = code,
                        purpose = purpose,
                        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                    },
                    actions = new[]
                    {
                        new
                        {
                            action = "verify",
                            title = "تحقق الآن"
                        },
                        new
                        {
                            action = "dismiss",
                            title = "تجاهل"
                        }
                    }
                };

                // إرسال Push Notification
                var success = await SendWebPushNotificationAsync(subscription, payload);

                if (success)
                {
                    _logger.LogInformation("Push notification verification code sent successfully for purpose {Purpose}", purpose);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send push notification verification code for purpose {Purpose}", purpose);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending push notification verification code for purpose {Purpose}", purpose);
                return false;
            }
        }

        /// <summary>
        /// إرسال Web Push Notification
        /// </summary>
        private async Task<bool> SendWebPushNotificationAsync(string subscription, object payload)
        {
            try
            {
                // تحويل الاشتراك من JSON
                var subscriptionData = JsonSerializer.Deserialize<PushSubscription>(subscription);
                
                if (subscriptionData == null)
                {
                    _logger.LogError("Invalid push subscription format");
                    return false;
                }

                // إنشاء VAPID JWT Token
                var jwtToken = GenerateVapidJwtToken(subscriptionData.endpoint);

                // إعداد Headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"vapid t={jwtToken}, k={_vapidPublicKey}");
                _httpClient.DefaultRequestHeaders.Add("TTL", "60"); // 60 seconds
                _httpClient.DefaultRequestHeaders.Add("Urgency", "high");

                // إرسال الطلب
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(subscriptionData.endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Web push notification sent successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to send web push notification. Status: {Status}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending web push notification");
                return false;
            }
        }

        /// <summary>
        /// إنشاء VAPID JWT Token
        /// </summary>
        private string GenerateVapidJwtToken(string endpoint)
        {
            // هذا مثال مبسط - في التطبيق الحقيقي تحتاج مكتبة JWT
            var header = new
            {
                typ = "JWT",
                alg = "ES256"
            };

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var payload = new
            {
                aud = new Uri(endpoint).GetLeftPart(UriPartial.Authority),
                exp = now + 12 * 3600, // 12 hours
                sub = "mailto:admin@endowment.com"
            };

            // في التطبيق الحقيقي، استخدم مكتبة JWT لإنشاء التوكن
            return "vapid.jwt.token.placeholder";
        }

        /// <summary>
        /// إنشاء رسالة Push Notification
        /// </summary>
        private string GeneratePushMessage(string code, string purpose)
        {
            var purposeText = purpose switch
            {
                "EmailConfirmation" => "تأكيد البريد الإلكتروني",
                "PasswordReset" => "إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "المصادقة الثنائية",
                _ => "التحقق"
            };

            return $"رمز التحقق الخاص بك لـ {purposeText} هو: {code}\nصالح لمدة 15 دقيقة فقط";
        }

        // Implementation of other interface methods (for compatibility)
        public async Task<bool> SendVerificationCodeViaEmailAsync(string email, string code, string purpose)
        {
            _logger.LogWarning("Email sending not supported by PushNotificationService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("SMS sending not supported by PushNotificationService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose)
        {
            _logger.LogWarning("Telegram sending not supported by PushNotificationService");
            return false;
        }

        public async Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose)
        {
            _logger.LogWarning("WhatsApp sending not supported by PushNotificationService");
            return false;
        }
    }

    /// <summary>
    /// نموذج اشتراك Push Notification
    /// </summary>
    public class PushSubscription
    {
        public string endpoint { get; set; } = string.Empty;
        public string p256dh { get; set; } = string.Empty;
        public string auth { get; set; } = string.Empty;
    }
}
