using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Application.Models.Identity
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            //builder.HasKey(e => e.UserId);
            builder.Property(e => e.Token).HasMaxLength(150);
            builder.HasIndex(e => e.Token).IsUnique();

            //builder.HasOne(e => e.User)
            //    .WithMany(c => c.Tokens)
            //    .HasForeignKey(e => e.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
