using Microsoft.AspNetCore.Identity;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence
{
    public class ApplicationUser : IdentityUser , IApplicationUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }
}