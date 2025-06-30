using Microsoft.Extensions.Configuration;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence
{
    public class DbContextFactory
    {
        private readonly string _connectionString;

        public DbContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }
}