using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;
using SpecificSolutions.Endowment.Core.Entities.Accounts;
using SpecificSolutions.Endowment.Core.Entities.AuditLogs;
using SpecificSolutions.Endowment.Core.Entities.Banks;
using SpecificSolutions.Endowment.Core.Entities.Branchs;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Core.Entities.Cities;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;
using SpecificSolutions.Endowment.Core.Entities.Decisions;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;
using SpecificSolutions.Endowment.Core.Entities.EndowmentExpenditureChangeRequests;
using SpecificSolutions.Endowment.Core.Entities.Facilities;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;
using SpecificSolutions.Endowment.Core.Entities.Mosques;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Entities.Products;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;
using SpecificSolutions.Endowment.Core.Entities.Regions;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence
{
    //public class appdbcontext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    //    IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<string>,
    //    IdentityRoleClaim<Guid>, RefreshToken>
    //{

    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
            ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, RefreshToken>
    {

        //public AppDbContext() { }

        //private static readonly string _connectionString = "server=. ; database=Endowment; integrated security=true;  trustservercertificate=true;";

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Configure the composite key for ApplicationUserRole
            //modelBuilder.Entity<ApplicationUserRole>(entity =>
            //{
            //    entity.HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key

            //    entity.Property(e => e.Permissions)
            //        .HasConversion(
            //            v => Helper.Serialize(v), // Convert to string for storage
            //            v => Helper.Deserialize<Permission>(v) // Convert back to internal format when reading
            //        );

            //    entity.HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key

            //    entity.HasOne(ur => ur.User)
            //        .WithMany(u => u.UserRoles)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();

            //    entity.HasOne(ur => ur.Role)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.RoleId)
            //        .IsRequired();
            //});

            modelBuilder.Entity<Decision>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Decisions)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // relationship between Request and ApplicationUser
            modelBuilder.Entity<Request>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Requests)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // relationship between Account and ApplicationUser
            modelBuilder.Entity<Account>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Accounts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefreshToken>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.IdentityUserTokens)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOne<Office>()
            //    .WithMany()
            //    .HasForeignKey(c => c.OfficeId);

            modelBuilder.Entity<Office>()
                .HasMany<ApplicationUser>()
                .WithOne(u => u.Office)
                .HasForeignKey(c => c.OfficeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Building>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Buildings)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<RefreshToken>()
            //    .ToTable("RefreshTokens")
            //    .HasBaseType<IdentityUserToken<string>>(); // This specifies the base type

            //modelBuilder.Entity<RefreshToken>(entity =>
            //{
            //    entity.HasKey(r => r.Id); // Adjust the key if necessary

            //    entity.HasOne(r => r.User)
            //        .WithMany(u => u.Tokens)
            //        .HasForeignKey(r => r.UserId)
            //        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading issues
            //});

            modelBuilder.ApplyConfiguration(new RequestConfig());
            modelBuilder.ApplyConfiguration(new DecisionConfig());
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AuditLogsConfiguration());
            //modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfig());
            //modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            //modelBuilder.ApplyConfiguration(new ApplicationRoleConfig());
            modelBuilder.ApplyConfiguration(new MosqueConfig());
            modelBuilder.ApplyConfiguration(new ConstructionRequestConfig());
            modelBuilder.ApplyConfiguration(new MaintenanceRequestConfig());
            modelBuilder.ApplyConfiguration(new ChangeOfPathRequestConfig());
            modelBuilder.ApplyConfiguration(new NameChangeRequestConfig());
            modelBuilder.ApplyConfiguration(new ExpenditureChangeRequestConfig());
            modelBuilder.ApplyConfiguration(new NeedsRequestConfig());
            modelBuilder.ApplyConfiguration(new DemolitionRequestConfig());
            modelBuilder.ApplyConfiguration(new QuranicSchoolConfig());

            modelBuilder.ApplyConfiguration(new FacilityConfig());
            modelBuilder.ApplyConfiguration(new BuildingDetailConfig());
            modelBuilder.ApplyConfiguration(new CityConfig());
            modelBuilder.ApplyConfiguration(new RegionConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new BankConfig());
            modelBuilder.ApplyConfiguration(new BranchConfig());
            modelBuilder.ApplyConfiguration(new BuildingDetailRequestConfig());
            modelBuilder.ApplyConfiguration(new OfficeConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new AccountDetailConfig());
            modelBuilder.ApplyConfiguration(new BuildingConfig());
            modelBuilder.ApplyConfiguration(new FacilityDetailConfig());
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountDetail> AccountDetails { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<RefreshToken> IdentityUserToken { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Mosque> Mosques { get; set; }
        public DbSet<FacilityDetail> FacilityDetails { get; set; }
        public DbSet<ConstructionRequest> ConstructionRequests { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<ChangeOfPathRequest> ChangeOfPathRequests { get; set; }
        public DbSet<NameChangeRequest> NameChangeRequests { get; set; }
        public DbSet<ExpenditureChangeRequest> ExpenditureChangeRequests { get; set; }
        public DbSet<NeedsRequest> NeedsRequests { get; set; }
        public DbSet<DemolitionRequest> DemolitionRequests { get; set; }
        public DbSet<QuranicSchool> QuranicSchools { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<BuildingDetail> BuildingDetails { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BuildingDetailRequest> BuildingDetailRequests { get; set; }
    }
}