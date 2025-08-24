using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Exceptions;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    /// <summary>
    /// خدمة المصادقة الرئيسية
    /// </summary>
    public class Authenticator : IAuthenticator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUser _currentUser;
        private readonly IOptions<JwtSettings> _jwtSettings;

        private readonly RegistrationService _registrationService;
        private readonly PasswordService _passwordService;
        private readonly PermissionService _permissionService;

        public Authenticator(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISessionService sessionService,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUser currentUser,
            IOptions<JwtSettings> jwtSettings,
            RegistrationService registrationService,
            PasswordService passwordService,
            PermissionService permissionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            _tokenService = tokenService;
            _currentUser = currentUser;
            _jwtSettings = jwtSettings;
            _registrationService = registrationService;
            _passwordService = passwordService;
            _permissionService = permissionService;
        }

        #region Login Operations

        /// <summary>
        /// تسجيل دخول المستخدم
        /// </summary>
        public async Task<IUserLogin> LoginAsync(LoginCommand command)
        {
            try
            {
                var user = await FindUserByEmailAsync(command.Email);

                if (!user.EmailConfirmed)
                {
                    throw new UnauthorizedAccessException("يرجى تأكيد بريدك الإلكتروني قبل تسجيل الدخول. تحقق من صندوق الوارد الخاص بك.");
                }

                if (!user.IsUserApproved())
                {
                    throw new UnauthorizedAccessException("حسابك في انتظار موافقة المسؤول. يرجى المحاولة لاحقاً.");
                }

                await SignInUserAsync(user, command.Password);
                ValidateHttpContext();

                var token = await _tokenService.GenerateTokenAsync(user);
                var refreshToken = _tokenService.GenerateRefreshTokenAsync();

                await CreateUserSessionAsync(user);
                var permissions = await _permissionService.GetUserPermissionsAsync(user);

                return CreateUserLoginResponse(user, token, refreshToken, permissions);
            }
            catch (NotFoundException e)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred during login.", ex);
            }
        }

        /// <summary>
        /// تسجيل خروج المستخدم
        /// </summary>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            await _sessionService.EndSessionAsync();
        }

        #endregion

        #region Registration Operations

        /// <summary>
        /// تسجيل مستخدم جديد (RegisterCommand)
        /// </summary>
        public async Task<RegistrationResponse> Register(RegisterCommand request)
        {
            return await _registrationService.RegisterAsync(request);
        }

        /// <summary>
        /// تسجيل مستخدم جديد (RegistrationRequest)
        /// </summary>
        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var command = new RegisterCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.Password,
                PhoneNumber = string.Empty,
                Address = string.Empty,
                City = string.Empty,
                Country = string.Empty,
                OfficeId = request.OfficeId.ToString(),
                IsApproved = request.IsApproved,
                ApprovedAt = request.ApprovedAt,
                ApprovedBy = request.ApprovedBy
            };
            
            return await _registrationService.RegisterAsync(command);
        }

        #endregion

        #region Permission Operations

        /// <summary>
        /// الحصول على أدوار المستخدم
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            return await _permissionService.GetUserRolesAsync(userId);
        }

        /// <summary>
        /// التحقق من وجود المستخدم في دور معين
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(string roleName)
        {
            return await _permissionService.IsUserInRoleAsync(roleName);
        }

        #endregion

        #region Password Operations

        /// <summary>
        /// نسيان كلمة المرور
        /// </summary>
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            return await _passwordService.ForgotPasswordAsync(email);
        }

        /// <summary>
        /// إعادة تعيين كلمة المرور
        /// </summary>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            return await _passwordService.ResetPasswordAsync(email, token, newPassword);
        }

        /// <summary>
        /// تغيير كلمة المرور
        /// </summary>
        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            return await _passwordService.ChangePasswordAsync(currentPassword, newPassword);
        }

        /// <summary>
        /// تأكيد البريد الإلكتروني
        /// </summary>
        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            return await _passwordService.ConfirmEmailAsync(email, token);
        }

        /// <summary>
        /// إعادة إرسال بريد تأكيد البريد الإلكتروني
        /// </summary>
        public async Task<bool> ResendEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("البريد الإلكتروني غير مسجل في النظام.");
            }

            if (user.EmailConfirmed)
            {
                throw new InvalidOperationException("البريد الإلكتروني مؤكد بالفعل.");
            }

            await _registrationService.SendEmailConfirmationAsync(user);
            return true;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// البحث عن المستخدم بالبريد الإلكتروني
        /// </summary>
        private async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            return user;
        }

        /// <summary>
        /// تسجيل دخول المستخدم
        /// </summary>
        private async Task SignInUserAsync(ApplicationUser user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid login attempt. Please check your credentials.");
            }
        }

        /// <summary>
        /// التحقق من وجود HttpContext
        /// </summary>
        private void ValidateHttpContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available. This method should be called in an HTTP context.");
            }
        }

        /// <summary>
        /// إنشاء جلسة المستخدم
        /// </summary>
        private async Task CreateUserSessionAsync(ApplicationUser user)
        {
            await _sessionService.CreateSessionAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: true);
        }

        /// <summary>
        /// إنشاء استجابة تسجيل الدخول
        /// </summary>
        private static UserLogin CreateUserLoginResponse(ApplicationUser user, string token, string refreshToken, List<string> permissions)
        {
            return new UserLogin
            {
                Id = user.Id,
                Name = string.IsNullOrWhiteSpace(user.Name)
                    ? ($"{user.FirstName} {user.LastName}".Trim())
                    : user.Name,
                Token = token,
                RefreshToken = refreshToken,
                Permissions = permissions
            };
        }

        #endregion
    }
}