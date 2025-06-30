using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Infrastructure;
using SpecificSolutions.Endowment.Infrastructure.Authentications.Services;

namespace SpecificSolutions.Endowment.Management
{
    public static class ApiContainer
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register API controllers
            services.AddControllers();

            services.AddSingleton<IUserLogin, UserLogin>();
            // Register services from the Infrastructure layer 
            services.AddInfrastructureServices(configuration);
            //TODO use other concret if need
            services.AddScoped<IAuthenticator, Authenticator>();

            // Register services from the Application layer
            services.AddApplicationServices();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
