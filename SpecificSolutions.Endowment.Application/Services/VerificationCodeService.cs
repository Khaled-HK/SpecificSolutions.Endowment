using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SpecificSolutions.Endowment.Application.Services
{
    public class VerificationCodeService
    {
        private readonly IVerificationCodeRepository _verificationCodeRepository;
        private readonly IEmailService _emailService;
        private readonly IMessagingService _messagingService;
        private readonly ILogger<VerificationCodeService> _logger;
        private readonly IConfiguration _configuration;

        public VerificationCodeService(
            IVerificationCodeRepository verificationCodeRepository,
            IEmailService emailService,
            IMessagingService messagingService,
            ILogger<VerificationCodeService> logger,
            IConfiguration configuration)
        {
            _verificationCodeRepository = verificationCodeRepository;
            _emailService = emailService;
            _messagingService = messagingService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// إنشاء رمز تحقق عشوائي من 6 أرقام
        /// </summary>
        public string GenerateCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// إنشاء رمز تحقق جديد وإرساله عبر البريد الإلكتروني
        /// </summary>
        public async Task<bool> SendVerificationCodeAsync(string email, string purpose, string? userId = null, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // التحقق من عدد الرموز النشطة
                var activeCodesCount = await _verificationCodeRepository.GetActiveCodesCountAsync(email, purpose);
                var maxCodesPerEmail = _configuration.GetValue<int>("VerificationCode:MaxCodesPerEmail", 3);

                if (activeCodesCount >= maxCodesPerEmail)
                {
                    _logger.LogWarning("User {Email} has reached maximum verification codes limit for purpose {Purpose}", email, purpose);
                    return false;
                }

                // إنشاء رمز جديد
                var code = GenerateCode();
                var expirationMinutes = _configuration.GetValue<int>("VerificationCode:ExpirationMinutes", 15);
                var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var verificationCode = new VerificationCode
                {
                    Email = email,
                    Code = code,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false,
                    IsExpired = false,
                    Purpose = purpose,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                // حفظ الرمز في قاعدة البيانات
                await _verificationCodeRepository.CreateAsync(verificationCode);

                // إرسال البريد الإلكتروني
                var emailContent = GenerateEmailContent(code, purpose, expirationMinutes);
                var subject = GetEmailSubject(purpose);
                
                var emailSent = await _emailService.SendEmailAsync(email, subject, emailContent);

                if (emailSent)
                {
                    _logger.LogInformation("Verification code sent successfully to {Email} for purpose {Purpose}", email, purpose);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send verification code email to {Email} for purpose {Purpose}", email, purpose);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending verification code to {Email} for purpose {Purpose}", email, purpose);
                return false;
            }
        }

        /// <summary>
        /// التحقق من صحة رمز التحقق
        /// </summary>
        public async Task<bool> VerifyCodeAsync(string email, string code, string purpose)
        {
            try
            {
                var verificationCode = await _verificationCodeRepository.GetValidCodeAsync(email, code, purpose);

                if (verificationCode == null)
                {
                    _logger.LogWarning("Invalid verification code attempt for {Email} with code {Code} for purpose {Purpose}", email, code, purpose);
                    return false;
                }

                // تحديث الرمز كمستخدم
                verificationCode.MarkAsUsed();
                await _verificationCodeRepository.UpdateAsync(verificationCode);

                _logger.LogInformation("Verification code verified successfully for {Email} for purpose {Purpose}", email, purpose);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying code for {Email} for purpose {Purpose}", email, purpose);
                return false;
            }
        }

        /// <summary>
        /// إعادة إرسال رمز التحقق
        /// </summary>
        public async Task<bool> ResendVerificationCodeAsync(string email, string purpose, string? userId = null, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // إلغاء الرموز السابقة
                await _verificationCodeRepository.InvalidatePreviousCodesAsync(email, purpose);

                // إرسال رمز جديد
                return await SendVerificationCodeAsync(email, purpose, userId, ipAddress, userAgent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resending verification code to {Email} for purpose {Purpose}", email, purpose);
                return false;
            }
        }

        /// <summary>
        /// حذف الرموز المنتهية الصلاحية
        /// </summary>
        public async Task<bool> CleanupExpiredCodesAsync()
        {
            try
            {
                var deleted = await _verificationCodeRepository.DeleteExpiredCodesAsync();
                _logger.LogInformation("Cleaned up {Count} expired verification codes", deleted);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up expired verification codes");
                return false;
            }
        }

        /// <summary>
        /// إرسال رمز تحقق عبر SMS
        /// </summary>
        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string purpose, string? userId = null, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // التحقق من عدد الرموز النشطة
                var activeCodesCount = await _verificationCodeRepository.GetActiveCodesCountAsync(phoneNumber, purpose);
                var maxCodesPerPhone = _configuration.GetValue<int>("VerificationCode:MaxCodesPerPhone", 3);

                if (activeCodesCount >= maxCodesPerPhone)
                {
                    _logger.LogWarning("User {PhoneNumber} has reached maximum verification codes limit for purpose {Purpose}", phoneNumber, purpose);
                    return false;
                }

                // إنشاء رمز جديد
                var code = GenerateCode();
                var expirationMinutes = _configuration.GetValue<int>("VerificationCode:ExpirationMinutes", 15);
                var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var verificationCode = new VerificationCode
                {
                    Email = phoneNumber, // استخدام حقل Email لتخزين رقم الهاتف
                    Code = code,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false,
                    IsExpired = false,
                    Purpose = purpose,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                // حفظ الرمز في قاعدة البيانات
                await _verificationCodeRepository.CreateAsync(verificationCode);

                // إرسال SMS
                var smsSent = await _messagingService.SendVerificationCodeViaSmsAsync(phoneNumber, code, purpose);

                if (smsSent)
                {
                    _logger.LogInformation("SMS verification code sent successfully to {PhoneNumber} for purpose {Purpose}", phoneNumber, purpose);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send SMS verification code to {PhoneNumber} for purpose {Purpose}", phoneNumber, purpose);
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
        /// إرسال رمز تحقق عبر Telegram
        /// </summary>
        public async Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string purpose, string? userId = null, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // التحقق من عدد الرموز النشطة
                var activeCodesCount = await _verificationCodeRepository.GetActiveCodesCountAsync(chatId, purpose);
                var maxCodesPerChat = _configuration.GetValue<int>("VerificationCode:MaxCodesPerChat", 3);

                if (activeCodesCount >= maxCodesPerChat)
                {
                    _logger.LogWarning("User {ChatId} has reached maximum verification codes limit for purpose {Purpose}", chatId, purpose);
                    return false;
                }

                // إنشاء رمز جديد
                var code = GenerateCode();
                var expirationMinutes = _configuration.GetValue<int>("VerificationCode:ExpirationMinutes", 15);
                var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var verificationCode = new VerificationCode
                {
                    Email = chatId, // استخدام حقل Email لتخزين Chat ID
                    Code = code,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false,
                    IsExpired = false,
                    Purpose = purpose,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                // حفظ الرمز في قاعدة البيانات
                await _verificationCodeRepository.CreateAsync(verificationCode);

                // إرسال Telegram
                var telegramSent = await _messagingService.SendVerificationCodeViaTelegramAsync(chatId, code, purpose);

                if (telegramSent)
                {
                    _logger.LogInformation("Telegram verification code sent successfully to {ChatId} for purpose {Purpose}", chatId, purpose);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send Telegram verification code to {ChatId} for purpose {Purpose}", chatId, purpose);
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
        /// إرسال رمز تحقق عبر Push Notification
        /// </summary>
        public async Task<bool> SendVerificationCodeViaPushNotificationAsync(string subscription, string purpose, string? userId = null, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // التحقق من عدد الرموز النشطة
                var activeCodesCount = await _verificationCodeRepository.GetActiveCodesCountAsync(subscription, purpose);
                var maxCodesPerSubscription = _configuration.GetValue<int>("VerificationCode:MaxCodesPerSubscription", 3);

                if (activeCodesCount >= maxCodesPerSubscription)
                {
                    _logger.LogWarning("User subscription has reached maximum verification codes limit for purpose {Purpose}", purpose);
                    return false;
                }

                // إنشاء رمز جديد
                var code = GenerateCode();
                var expirationMinutes = _configuration.GetValue<int>("VerificationCode:ExpirationMinutes", 15);
                var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var verificationCode = new VerificationCode
                {
                    Email = subscription, // استخدام حقل Email لتخزين Subscription
                    Code = code,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsUsed = false,
                    IsExpired = false,
                    Purpose = purpose,
                    UserId = userId,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                // حفظ الرمز في قاعدة البيانات
                await _verificationCodeRepository.CreateAsync(verificationCode);

                // إرسال Push Notification
                var pushSent = await _messagingService.SendVerificationCodeViaPushNotificationAsync(subscription, code, purpose);

                if (pushSent)
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
        /// إنشاء محتوى البريد الإلكتروني
        /// </summary>
        private string GenerateEmailContent(string code, string purpose, int expirationMinutes)
        {
            var purposeText = purpose switch
            {
                "EmailConfirmation" => "تأكيد البريد الإلكتروني",
                "PasswordReset" => "إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "المصادقة الثنائية",
                _ => "التحقق"
            };

            return $@"
<div dir='rtl' style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
    <div style='background-color: #f8f9fa; padding: 20px; border-radius: 10px; text-align: center;'>
        <h2 style='color: #333; margin-bottom: 20px;'>رمز التحقق</h2>
        
        <div style='background-color: #fff; padding: 30px; border-radius: 8px; border: 2px solid #e9ecef; margin: 20px 0;'>
            <h1 style='color: #007bff; font-size: 48px; margin: 0; letter-spacing: 10px; font-weight: bold;'>{code}</h1>
        </div>
        
        <p style='color: #666; font-size: 16px; line-height: 1.6;'>
            رمز التحقق الخاص بك لـ <strong>{purposeText}</strong> هو:
        </p>
        
        <div style='background-color: #e7f3ff; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p style='color: #0056b3; margin: 0; font-weight: bold;'>
                ⏰ هذا الرمز صالح لمدة {expirationMinutes} دقيقة فقط
            </p>
        </div>
        
        <div style='background-color: #fff3cd; padding: 15px; border-radius: 5px; margin: 20px 0;'>
            <p style='color: #856404; margin: 0; font-size: 14px;'>
                🔒 إذا لم تطلب هذا الرمز، يمكنك تجاهل هذا البريد الإلكتروني بأمان
            </p>
        </div>
        
        <hr style='border: none; border-top: 1px solid #dee2e6; margin: 30px 0;'>
        
        <p style='color: #999; font-size: 12px; margin: 0;'>
            شكراً لك،<br>
            فريق {_configuration["AppSettings:AppName"] ?? "نظام الأوقاف"}
        </p>
    </div>
</div>";
        }

        /// <summary>
        /// الحصول على عنوان البريد الإلكتروني حسب الغرض
        /// </summary>
        private string GetEmailSubject(string purpose)
        {
            return purpose switch
            {
                "EmailConfirmation" => "رمز تأكيد البريد الإلكتروني",
                "PasswordReset" => "رمز إعادة تعيين كلمة المرور",
                "TwoFactorAuth" => "رمز المصادقة الثنائية",
                _ => "رمز التحقق"
            };
        }
    }
}
