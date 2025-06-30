using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Contracts.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Infrastructure.Authentications.Services;
using SpecificSolutions.Endowment.Infrastructure.Persistence;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AccountDetails;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Accounts;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AuditLogs;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ConstructionRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Decisions;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.DemolitionRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.EndowmentExpenditureChangeRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Facilities;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.MaintenanceRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Mosques;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NameChangeRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NeedsRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Offices;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Requests;
using SpecificSolutions.Endowment.Infrastructure.Seeders;

namespace SpecificSolutions.Endowment.Infrastructure;

public static class InfrastructureContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        //TODO use iretrycaller pattren

        var dsd = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .EnableSensitiveDataLogging()
                   .LogTo(Console.WriteLine, LogLevel.Information));

        services.AddHostedService<DatabaseBuilder>();

        services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
        {
            o.User.RequireUniqueEmail = false;
            o.SignIn.RequireConfirmedEmail = false;
            o.Password.RequireDigit = false;
            o.Password.RequiredLength = 6;
            o.Password.RequiredUniqueChars = 1;
            o.Password.RequireLowercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
        .AddEntityFrameworkStores<AppDbContext>()
        //.AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, string>>()
        .AddDefaultTokenProviders();

        //services.Configure<IdentityOptions>(options =>
        //{
        //    options.User.RequireUniqueEmail = true;
        //    options.User.RequireUniqueEmail = true;       // Treat email as username

        //    options.User.AllowedUserNameCharacters = null; // Allow emails as usernames
        //});

        // Register Repositories   
        //TODO move it to api
        services.AddSingleton<ICurrentUser, CurrentUser>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        //services.AddScoped<ISerializerService, SerializerService>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IDecisionRepository, DecisionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountDetailRepository, AccountDetailRepository>();
        services.AddScoped<IAuditLogsRepository, AuditLogsRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Register DatabaseInitializer
        //services.AddScoped<IDatabaseBuilder, DatabaseBuilder>();

        // Register Office Repository
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IMosqueRepository, MosqueRepository>();

        services.AddScoped<IConstructionRequestRepository, ConstructionRequestRepository>();

        services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();

        services.AddScoped<IChangeOfPathRequestRepository, ChangeOfPathRequestRepository>();

        services.AddScoped<INameChangeRequestRepository, NameChangeRequestRepository>();

        services.AddScoped<IExpenditureChangeRequestRepository, ExpenditureChangeRequestRepository>();

        services.AddScoped<INeedsRequestRepository, NeedsRequestRepository>();

        services.AddScoped<IDemolitionRequestRepository, DemolitionRequestRepository>();

        services.AddScoped<IFacilityRepository, FacilityRepository>();

        return services;
    }
}

public class DatabaseBuilder : IHostedService
{
    private readonly IServiceProvider _provider;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseBuilder(IServiceProvider provider, IPasswordHasher passwordHasher)
    {
        _provider = provider;
        _passwordHasher = passwordHasher;

    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _provider.CreateScope();

        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await appDbContext.Database.MigrateAsync(cancellationToken);

        // Optionally, seed the database
        await Seeder.SeedAsync(appDbContext, _passwordHasher);

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
