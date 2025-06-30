using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Register;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity;

namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    public interface IAuthenticator
    {
        //Task<List<ApplicationUser>> GetUsers();
        Task<IUserLogin> LoginAsync(LoginCommand request);
        Task<RegistrationResponse> Register(RegistrationRequest request);
        Task<RegistrationResponse> Register(RegisterCommand request);
        Task LogoutAsync();
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> IsUserInRoleAsync(string roleName);
    }
}