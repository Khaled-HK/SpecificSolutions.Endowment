using Microsoft.AspNetCore.Identity;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    public class WpfAuthenticator : IAuthenticator
    {
        //inject UnitOfWork here if needed
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly ITokenService _tokenService;

        public WpfAuthenticator(IUnitOfWork unitOfWork, IPasswordHasher<ApplicationUser> passwordHasher, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public Task<List<string>> GetUserRolesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationUser>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        //public async Task<T> LoginAsync(LoginCommand command)
        //{
        //    var user = await _unitOfWork.LoginRepository.LoginAsync(command);
        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    // Handle session creation or other logic here

        //    return new UserLogin(user.Id, user.Name, user.Permissions);
        //}


        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<RegistrationResponse> Register(RegisterCommand request)
        {
            //TODO Implement Register
            throw new NotImplementedException();
        }

        public async Task<IUserLogin> LoginAsync(LoginCommand command)
        {
            var user = await _unitOfWork.Logins.LoginAsync(command);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            //TODO Update corrent user 

            var verificationResult = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, command.Password);

            if (verificationResult != PasswordVerificationResult.Success)
            {
                throw new Exception("Invalid login attempt.");
            }

            var refreshToken = _tokenService.GenerateRefreshTokenAsync();

            return new UserLogin("", user.Id, user.Name, refreshToken, user.Permissions);

        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            // For WPF application, this might not be needed or could be implemented differently
            // For now, we'll throw a NotImplementedException
            throw new NotImplementedException("ForgotPasswordAsync is not implemented for WPF application.");
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            // For WPF application, this might not be needed or could be implemented differently
            // For now, we'll throw a NotImplementedException
            throw new NotImplementedException("ResetPasswordAsync is not implemented for WPF application.");
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            // For WPF application, this might not be needed or could be implemented differently
            // For now, we'll throw a NotImplementedException
            throw new NotImplementedException("ChangePasswordAsync is not implemented for WPF application.");
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            // For WPF application, this might not be needed or could be implemented differently
            // For now, we'll throw a NotImplementedException
            throw new NotImplementedException("ConfirmEmailAsync is not implemented for WPF application.");
        }

        public Task<bool> ResendEmailConfirmationAsync(string email)
        {
            throw new NotImplementedException();
        }

        // Implement other methods as needed
    }
}