using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // تكوين الخصائص
            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);
            builder.Property(u => u.Name).HasMaxLength(200);
            builder.Property(u => u.OfficeId).IsRequired();

            // تكوين العلاقات
            builder.HasOne(u => u.Office)
                .WithMany()
                .HasForeignKey(u => u.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            //// بيانات البداية - مستخدم معلق للاختبار
            //var testUser = ApplicationUser.Create(
            //    email: "test.pending@gmail.com",
            //    firstName: "Test",
            //    lastName: "Pending",
            //    officeId: Guid.Parse("ddec6e9e-7628-4623-9a94-4e4efc02187c"), // نفس OfficeId المستخدم
            //    userName: "test.pending@gmail.com",
            //    passwordHash: "AQAAAAIAAYagAAAAELbXp1J7mVDkbJPRFemQDF/58cQm3oqqJjdDa8gMcdfKmc2oB6MmLtKjQp6Xy6I0Sw==", // كلمة مرور: Test123!
            //    emailConfirmed: true
            //);

            //// التأكد من أن المستخدم غير معتمد
            //testUser.RejectUser();

            //builder.HasData(testUser);
        }
    }
}
