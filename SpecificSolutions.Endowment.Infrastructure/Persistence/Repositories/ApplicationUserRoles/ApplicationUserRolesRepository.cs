using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationUserRoles
{
    internal class ApplicationUserRolesRepository : Repository<ApplicationUserRole>, IApplicationUserRolesRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRolesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
