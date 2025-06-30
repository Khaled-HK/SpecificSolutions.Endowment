using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Infrastructure.Seeders;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence
{
    public class DatabaseBuilder : IDatabaseBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        //TODO use build in and improve class
        private readonly IPasswordHasher _passwordHasher;

        public DatabaseBuilder(
            IServiceProvider serviceProvider,
            IPasswordHasher passwordHasher)
        {
            _serviceProvider = serviceProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Apply any pending migrations
                await context.Database.MigrateAsync();

                // Optionally, seed the database
                await Seeder.SeedAsync(context, _passwordHasher);
            }
        }
    }
}