namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    /// <summary>
    /// واجهة مشتركة لخدمات إرسال الرسائل (Email, SMS, Telegram, WhatsApp, Push Notifications)
    /// </summary>
    public interface IMessagingService
    {
        /// <summary>
        /// إرسال رمز تحقق عبر البريد الإلكتروني
        /// </summary>
        Task<bool> SendVerificationCodeViaEmailAsync(string email, string code, string purpose);

        /// <summary>
        /// إرسال رمز تحقق عبر SMS
        /// </summary>
        Task<bool> SendVerificationCodeViaSmsAsync(string phoneNumber, string code, string purpose);

        /// <summary>
        /// إرسال رمز تحقق عبر Telegram
        /// </summary>
        Task<bool> SendVerificationCodeViaTelegramAsync(string chatId, string code, string purpose);

        /// <summary>
        /// إرسال رمز تحقق عبر WhatsApp
        /// </summary>
        Task<bool> SendVerificationCodeViaWhatsAppAsync(string phoneNumber, string code, string purpose);

        /// <summary>
        /// إرسال رمز تحقق عبر Push Notifications
        /// </summary>
        Task<bool> SendVerificationCodeViaPushNotificationAsync(string subscription, string code, string purpose);
    }
}
