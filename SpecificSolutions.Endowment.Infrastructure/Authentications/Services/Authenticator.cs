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
    //TODO refactor
    public class Authenticator : IAuthenticator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject ApplicationUserToken here
        // private readonly ApplicationUserToken _applicationUserToken;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionService _sessionService;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUser _currentUser;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public Authenticator(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             ISessionService sessionService,
                             ITokenService tokenService,
                             IHttpContextAccessor httpContextAccessor,
                             ICurrentUser currentUser,
                             IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _sessionService = sessionService;
            _tokenService = tokenService;
            _currentUser = currentUser;
            _jwtSettings = jwtSettings;
        }

        public async Task<IUserLogin> LoginAsync(LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            //user.SecurityStamp = Guid.NewGuid().ToString();

            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //{
            //    throw new Exception("Email not confirmed.");
            //}

            //var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var result1 = await _userManager.ResetPasswordAsync(user, resetToken, "1");

            var result = await _signInManager.PasswordSignInAsync(user, command.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid login attempt.");
            }

            // Check if HttpContext is available
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available. This method should be called in an HTTP context.");
            }

            var token = await _tokenService.GenerateTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshTokenAsync();

            // set user signin status in HttpContext with Cookie Authentication

            await _sessionService.CreateSessionAsync(user);
            await _signInManager.SignInAsync(user, isPersistent: true);

            //await _signInManager.SignInAsync(user, isPersistent: true);

            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    userPrincipal,
            //    new AuthenticationProperties
            //    {
            //        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
            //        IsPersistent = true,
            //        AllowRefresh = true
            //    });

            // Get permissions from user roles
            var roles = await _userManager.GetRolesAsync(user);
            user.AddPermissions(roles.ToList());

            await _sessionService.CreateSessionAsync(user);

            var response = new UserLogin(
                token: token,
                id: user.Id,
                name: user.Name,
                refreshToken: refreshToken,
                permissions: user.Permissions
            );

            return response;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
            {
                throw new Exception($"User {request.UserName} already exists");
            }

            var user = ApplicationUser.Create(request.Email, request.FirstName, request.LastName,
                request.OfficeId != Guid.Empty ? request.OfficeId : new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"), request.UserName, request.Password, true);

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return new RegistrationResponse() { UserId = user.Id.ToString() };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Registration failed: {errors}");
                }
            }
            else
            {
                throw new Exception($"Email {request.Email} already exists.");
            }
        }

        public async Task<RegistrationResponse> Register(RegisterCommand request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
            {
                throw new Exception($"User {request.UserName} already exists");
            }

            var user = ApplicationUser.Create(request.Email, request.FirstName, request.LastName,
                Guid.Parse(request.OfficeId), request.UserName, request.Password, true);

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return new RegistrationResponse() { UserId = user.Id.ToString() };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Registration failed: {errors}");
                }
            }
            else
            {
                throw new Exception($"Email {request.Email} already exists.");
            }
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Select(q => q.Value).ToList();
        }

        public async Task<bool> IsUserInRoleAsync(string roleName)
        {
            ApplicationUser? applicationUser = await _sessionService.GetUser();
            if (applicationUser == null)
            {
                return false;
            }

            var u = applicationUser.Permissions.Any(q => q == roleName);

            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return await _userManager.IsInRoleAsync(applicationUser, roleName);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            await _sessionService.EndSessionAsync();
        }
    }
}