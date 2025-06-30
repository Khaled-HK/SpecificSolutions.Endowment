using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationRoles
{
    internal class ApplicationRoleRepository : Repository<ApplicationRole>, IApplicationRoleRepository
    {
        private readonly AppDbContext _context;
        public ApplicationRoleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
