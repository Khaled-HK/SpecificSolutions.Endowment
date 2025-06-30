using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface ILoginRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> LoginAsync(LoginCommand command);
        //Task<bool> LoginAsync(LoginCommand command);
    }
}
