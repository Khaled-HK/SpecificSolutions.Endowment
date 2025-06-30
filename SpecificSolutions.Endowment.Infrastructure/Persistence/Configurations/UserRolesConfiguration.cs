using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Core.Helpers;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key

            builder.Property(e => e.Permissions)
                .HasConversion(
                    v => Helper.Serialize(v), // Convert to string for storage
                    v => Helper.Deserialize<Permission>(v) // Convert back to internal format when reading
                );

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoles)
                   .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId);
        }
    }
}
