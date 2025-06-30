using Microsoft.Extensions.DependencyInjection;

namespace SpecificSolutions.Endowment.Core
{
    public static class CoreContainer
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Register core services, domain services, or any other necessary components here
            // Example: services.AddTransient<IMyDomainService, MyDomainService>();

            return services;
        }
    }
}