using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Infrastructure;
using SpecificSolutions.Endowment.Infrastructure.Authentications.Services;
using System.Globalization;
using System.Windows;

namespace SpecificSolutions.Endowment.Wpf;


public partial class App : System.Windows.Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        // Configure services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();

        //ToDO : get the language from the header if form wpf get the language from the current thread culture
        // get culture from the properties settings by name "language"
        //var language = Properties.Settings.Default.language;
        //var culture = new CultureInfo(language);
        //CultureInfo.CurrentCulture = culture;
        //CultureInfo.CurrentUICulture = culture;

        // Set the culture for the current thread
        var deviceCulture = CultureInfo.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = deviceCulture;
        Thread.CurrentThread.CurrentUICulture = deviceCulture;

        //TODO but it in other place
        // Initialize the database
        using (var scope = _serviceProvider.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            ApplicationContainer.InitializeDatabaseAsync(serviceProvider).GetAwaiter().GetResult();
        }
    }

    private void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        services.AddInfrastructureServices(configuration);
        //TODO
        services.AddScoped<IAuthenticator, WpfAuthenticator>();

        services.AddApplicationServices();

        // Register ViewModels
        services.AddTransient<AccountViewModel>(); // Register your ViewModel

        // Register Views
        services.AddTransient<AccountView>(); // Register your AccountView

    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var accountView = _serviceProvider.GetRequiredService<AccountView>(); // Resolve the AccountView
        accountView.Show();
    }
}