using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Core.Models;
using SpecificSolutions.Endowment.Infrastructure.Persistence;
using SpecificSolutions.Endowment.Infrastructure.Services;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    public class RegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public RegistrationService(
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext,
            IEmailService emailService,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// تسجيل مستخدم جديد
        /// </summary>
        public async Task<RegistrationResponse> RegisterAsync(RegisterCommand request)
        {
            // التحقق من صحة البيانات
            await ValidateUserDoesNotExistAsync(request.UserName, request.Email);
            await ValidateEmailFormatAsync(request.Email);

            // معالجة OfficeId
            var officeId = !string.IsNullOrEmpty(request.OfficeId) && Guid.TryParse(request.OfficeId, out var parsedOfficeId)
                ? parsedOfficeId
                : new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"); // OfficeId افتراضي

            // إنشاء المستخدم
            var user = ApplicationUser.Create(
                request.Email,
                request.FirstName,
                request.LastName,
                officeId,
                request.UserName,
                request.Password,
                false,  // emailConfirmed = false - يحتاج تأكيد
                request.IsApproved,
                request.ApprovedAt,
                request.ApprovedBy
            );

            // إنشاء المستخدم في قاعدة البيانات
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            // إرسال بريد تأكيد البريد الإلكتروني
            await SendEmailConfirmationAsync(user);

            return new RegistrationResponse
            {
                UserId = user.Id,
                Message = "تم التسجيل بنجاح. يرجى التحقق من بريدك الإلكتروني لتأكيد الحساب.",
                RequiresEmailConfirmation = true  // ✅ الباك إند يحدد صراحةً
            };
        }

        /// <summary>
        /// التحقق من عدم وجود المستخدم مسبقاً
        /// </summary>
        private async Task ValidateUserDoesNotExistAsync(string userName, string email)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                throw new Exception($"User {userName} already exists");
            }

            var existingEmail = await _userManager.FindByEmailAsync(email);
            if (existingEmail != null)
            {
                throw new Exception($"Email {email} already exists");
            }
        }

        /// <summary>
        /// التحقق من صحة تنسيق البريد الإلكتروني
        /// </summary>
        private async Task ValidateEmailFormatAsync(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                if (mailAddress.Address != email)
                {
                    throw new Exception("تنسيق البريد الإلكتروني غير صحيح");
                }
            }
            catch
            {
                throw new Exception("تنسيق البريد الإلكتروني غير صحيح");
            }
        }

        /// <summary>
        /// إرسال بريد تأكيد البريد الإلكتروني
        /// </summary>
        public async Task SendEmailConfirmationAsync(ApplicationUser user)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // استخدام الرابط الصحيح من الإعدادات
                var baseUrl = !string.IsNullOrEmpty(_appSettings.FrontendUrl)
                    ? _appSettings.FrontendUrl
                    : _appSettings.BaseUrl;

                var confirmationLink = $"{baseUrl}/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token)}";

                // إرسال بريد تأكيد البريد الإلكتروني
                var emailSent = await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);

                if (!emailSent)
                {
                    Console.WriteLine($"Failed to send confirmation email to {user.Email}");
                }
            }
            catch (Exception ex)
            {
                // لا نريد أن يفشل التسجيل بسبب مشكلة في إرسال البريد
                Console.WriteLine($"Failed to send confirmation email: {ex.Message}");
            }
        }

        // تم نقل methods إدارة الصلاحيات إلى UserApprovalService
        // RegistrationService مسؤول فقط عن التسجيل بدون صلاحيات
    }
}
