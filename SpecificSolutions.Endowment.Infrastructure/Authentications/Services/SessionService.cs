using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Authentications.Services
{
    //TODO use it for current user
    public class SessionService : ISessionService
    {
        // This could be a database or in-memory store
        private static readonly Dictionary<Guid, DateTime> _sessions = new();
        private static ApplicationUser? _user;

        public Task<ApplicationUser?> GetUser()
        {
            return Task.FromResult(_user);
        }

        public Task CreateSessionAsync(ApplicationUser user)
        {
            _user = user;
            return Task.CompletedTask;
        }

        public Task EndSessionAsync()
        {
            _user = null;
            // Logic to end the session (e.g., remove from store)
            // This could be more complex depending on your requirements
            return Task.CompletedTask;
        }

        public Task GetSessionAsync()
        {

            return Task.CompletedTask;
        }
    }
}