namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    /// <summary>
    /// واجهة خدمة إرسال البريد الإلكتروني
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// إرسال بريد إلكتروني
        /// </summary>
        Task<bool> SendEmailAsync(string to, string subject, string body);

        /// <summary>
        /// إرسال بريد تأكيد البريد الإلكتروني
        /// </summary>
        Task<bool> SendEmailConfirmationAsync(string email, string confirmationLink);

        /// <summary>
        /// إرسال بريد إعادة تعيين كلمة المرور
        /// </summary>
        Task<bool> SendPasswordResetAsync(string email, string resetLink);

        /// <summary>
        /// التحقق من صحة البريد الإلكتروني
        /// </summary>
        Task<bool> ValidateEmailAsync(string email);
    }
}
