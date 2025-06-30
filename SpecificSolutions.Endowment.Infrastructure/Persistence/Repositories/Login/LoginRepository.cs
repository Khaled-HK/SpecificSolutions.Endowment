using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.Login;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Login
{
    public class LoginRepository : Repository<ApplicationUser>, ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> LoginAsync(LoginCommand command)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == command.Email);
        }
    }
}