using Microsoft.AspNetCore.Identity;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    /// <summary>
    /// خدمة إدارة كلمات المرور - مسؤولة فقط عن عمليات كلمات المرور
    /// </summary>
    public class PasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISessionService _sessionService;
        private readonly IEmailService _emailService;

        public PasswordService(UserManager<ApplicationUser> userManager, ISessionService sessionService, IEmailService emailService)
        {
            _userManager = userManager;
            _sessionService = sessionService;
            _emailService = emailService;
        }

        /// <summary>
        /// نسيان كلمة المرور - إرسال رمز الاستعادة
        /// </summary>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found with this email address.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // إرسال بريد إعادة تعيين كلمة المرور
            var resetLink = $"https://yourapp.com/reset-password?email={email}&token={token}";
            var emailSent = await _emailService.SendPasswordResetAsync(email, resetLink);
            
            if (!emailSent)
            {
                throw new Exception("Failed to send password reset email.");
            }
            
            return true;
        }

        /// <summary>
        /// إعادة تعيين كلمة المرور باستخدام الرمز
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found with this email address.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {errors}");
            }

            return true;
        }

        /// <summary>
        /// تغيير كلمة المرور للمستخدم المسجل دخوله
        /// </summary>
        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = await _sessionService.GetUser();
            if (user == null)
            {
                throw new Exception("User not authenticated.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password change failed: {errors}");
            }

            return true;
        }

        /// <summary>
        /// تأكيد البريد الإلكتروني
        /// </summary>
        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found with this email address.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Email confirmation failed: {errors}");
            }

            return true;
        }
    }
}
