using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Core.Models;
using SpecificSolutions.Endowment.Infrastructure.Services;

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
        private readonly AppSettings _appSettings;

        public PasswordService(UserManager<ApplicationUser> userManager, ISessionService sessionService, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _sessionService = sessionService;
            _emailService = emailService;
            _appSettings = appSettings.Value;
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
            var baseUrl = !string.IsNullOrEmpty(_appSettings.FrontendUrl) 
                ? _appSettings.FrontendUrl 
                : _appSettings.BaseUrl;
            var resetLink = $"{baseUrl}/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
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
