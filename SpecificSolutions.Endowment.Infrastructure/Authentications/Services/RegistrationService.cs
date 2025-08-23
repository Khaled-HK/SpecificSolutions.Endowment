using Microsoft.AspNetCore.Identity;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    /// <summary>
    /// خدمة التسجيل - مسؤولة فقط عن تسجيل المستخدمين الجدد
    /// </summary>
    public class RegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public RegistrationService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// تسجيل مستخدم جديد مع الدور والصلاحيات
        /// </summary>
        public async Task<RegistrationResponse> RegisterAsync(RegisterCommand request)
        {
            // التحقق من عدم وجود المستخدم
            await ValidateUserDoesNotExistAsync(request.UserName, request.Email);

            // التحقق من صحة البريد الإلكتروني
            await ValidateEmailFormatAsync(request.Email);

            // إنشاء المستخدم
            var user = ApplicationUser.Create(
                request.Email, 
                request.FirstName, 
                request.LastName,
                Guid.Parse(request.OfficeId), 
                request.UserName, 
                request.Password, 
                false  // emailConfirmed = false - يحتاج تأكيد
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
                Message = "تم التسجيل بنجاح. يرجى تأكيد بريدك الإلكتروني ثم انتظار موافقة المسؤول."
            };
        }

        /// <summary>
        /// تسجيل مستخدم جديد (النسخة الثانية)
        /// </summary>
        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            // التحقق من عدم وجود المستخدم
            await ValidateUserDoesNotExistAsync(request.UserName, request.Email);

            // معالجة OfficeId الافتراضي
            var officeId = request.OfficeId != Guid.Empty 
                ? request.OfficeId 
                : new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C");

            // إنشاء المستخدم
            var user = ApplicationUser.Create(
                request.Email,
                request.FirstName,
                request.LastName,
                officeId,
                request.UserName,
                request.Password,
                true
            );

            // إنشاء المستخدم في قاعدة البيانات
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            // لا نضيف صلاحيات - المستخدم بحاجة لموافقة المسؤول
            // المستخدم سيكون IsApproved = false افتراضياً

            return new RegistrationResponse 
            { 
                UserId = user.Id,
                Message = "تم التسجيل بنجاح. في انتظار موافقة المسؤول لتفعيل الحساب."
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
        private async Task SendEmailConfirmationAsync(ApplicationUser user)
        {
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://yourapp.com/confirm-email?email={user.Email}&token={token}";
                
                // TODO: إرسال البريد الإلكتروني فعلياً
                // await _emailService.SendEmailAsync(user.Email, "تأكيد البريد الإلكتروني", confirmationLink);
                
                Console.WriteLine($"Email confirmation link: {confirmationLink}");
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
