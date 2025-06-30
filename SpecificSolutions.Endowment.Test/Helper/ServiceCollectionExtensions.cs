using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Core.Entities.Cities;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Entities.Regions;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SpecificSolutions.Endowment.Test.Helper;

public static class ServiceCollectionExtensions
{
    public static void SetUnitTestsDefaultEnvironment(this
        IServiceCollection services,
        IServiceProvider serviceProvider,
        IAuthenticator authenticator = null,
        ICurrentUser currentUser = null)
    {
        //#pragma warning disable CS0162 // Unreachable code detected
        //            if (TestConfig.UseSqlDatabase)
        UseSqlDatabaseTesting(services, serviceProvider);
        //else
        //    UseInMemoryTesting(services);

        UseMockService(services, currentUser);

    }

    public static void UseMockService(IServiceCollection services, ICurrentUser? currentUser = null)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(ICurrentUser));

        services.Remove(descriptor);

        if (currentUser != null)
        {
            services.AddSingleton(currentUser);
        }
        else
        {
            var mockGrpcService = new Mock<ICurrentUser>();

            services.AddSingleton(mockGrpcService.Object);
        }
    }

    private static void UseInMemoryTesting(IServiceCollection services)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

        services.Remove(descriptor);

        var dbName = Guid.NewGuid().ToString();

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(dbName));
    }

    private static void UseSqlDatabaseTesting(IServiceCollection services, IServiceProvider serviceProvider)
    {
        var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        //var descriptor1 = services.Single(d => d.ServiceType == typeof(IDatabaseBuilder));

        services.Remove(descriptor);
        //services.Remove(descriptor1);

        //services.AddDbContext<AppDbContext>(options =>
        //{
        //    options.UseSqlServer("Server=.;Database=Endowment_test;Trusted_Connection=True;MultipleActiveResultSets=true;trustservercertificate=true");
        //});

        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer("Server=.;Database=Endowment_test;Trusted_Connection=True;MultipleActiveResultSets=true;trustservercertificate=true")
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));
        //ResetDatabase(services.BuildServiceProvider());
        services.AddHostedService<DbTruncate>();

        //SeedDatabaseAsync(serviceProvider);
    }

    private static void SeedDatabaseAsync(IServiceProvider _provider)
    {
        var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        context.Accounts.RemoveRange(context.Accounts);
        context.Offices.RemoveRange(context.Offices);
        context.Requests.RemoveRange(context.Requests);
        context.ApplicationUser.RemoveRange(context.ApplicationUser);

        context.SaveChanges();
        context.ChangeTracker.Clear();

        context.Offices.Add(Office.Seed(
            id: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
            userId: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc"),
            name: "Main Office",
            location: "Tripoli",
            phoneNumber: "09284746974",
            regionId: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C")));

        context.Users.Add(ApplicationUser.Seed(new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc").ToString(),
                                            "1",
                                            "Khaled111",
                                            "Alnefati222",
                                            new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                                            "1",
                                            "1",
                                            true));

        context.SaveChanges();
    }

    //public static void ResetDatabase(this IServiceProvider serviceProvider)
    //{
    //    using (var scope = serviceProvider.CreateScope())
    //    {
    //        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //        dbContext.Database.EnsureDeleted();
    //        dbContext.Database.EnsureCreated();
    //    }
    //}
}

public class DbTruncate : IHostedService
{
    private readonly IServiceProvider _provider;

    public DbTruncate(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync(cancellationToken);

        context.Accounts.RemoveRange(context.Accounts);
        context.Offices.RemoveRange(context.Offices);
        context.Requests.RemoveRange(context.Requests);
        context.AuditLogs.RemoveRange(context.AuditLogs);
        context.ApplicationUser.RemoveRange(context.ApplicationUser);
        context.Regions.RemoveRange(context.Regions);
        context.Cities.RemoveRange(context.Cities);

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();

        context.Cities.Add(City.Seed(
                    id: new Guid("C658312F-1519-443F-8E1F-12307F01B77D"),
                    name: "Tripoli",
                    country: "Libya"));

        context.Regions.Add(Region.Seed(
                    id: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C"),
                    cityId: new Guid("C658312F-1519-443F-8E1F-12307F01B77D"),
                    name: "Tripoli",
                    country: "Libya"));

        context.Offices.Add(Office.Seed(
           id: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
           userId: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc"),
           name: "Main Office",
           location: "Tripoli",
           phoneNumber: "09284746974",
           regionId: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C")));

        context.Users.Add(ApplicationUser.Seed(new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc").ToString(),
                                            "1",
                                            "Khaled111",
                                            "Alnefati222",
                                            new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
                                            "1",
                                            "1",
                                            true));

        await context.SaveChangesAsync(cancellationToken);

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

