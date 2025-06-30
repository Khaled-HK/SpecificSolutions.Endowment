using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application.Abstractions.Behaviors;
using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Validators;
using System.Reflection;

namespace SpecificSolutions.Endowment.Application
{
    public static class ApplicationContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddMediatR(m => m.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddLogging(); // Register ILogger

            services.AddApplicationValidators();

            //services.AddScoped<IRequestHandler<LoginCommand, EndowmentResponse<IUserLogin>>, LoginHandler>();

            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                m.AddOpenBehavior(typeof(AuthorizationPipelineBehavior<,>));
                m.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                m.AddOpenBehavior(typeof(LoggingPipelineBehaviour<,>));
                m.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
                m.AddOpenBehavior(typeof(RetryBehavior<,>));
            });

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            var databaseInitializer = serviceProvider.GetRequiredService<IDatabaseBuilder>();
            await databaseInitializer.InitializeAsync();
        }
    }
}