using Microsoft.AspNetCore.Identity;

namespace SpecificSolutions.Endowment.Application.Models.Identity.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
