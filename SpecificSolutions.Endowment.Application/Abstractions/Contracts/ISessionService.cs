using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    public interface ISessionService
    {
        Task CreateSessionAsync(ApplicationUser user);

        //get the current session
        Task EndSessionAsync();
        Task GetSessionAsync();
        Task<ApplicationUser?> GetUser();
    }
}