using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Infrastructure.Persistence;

namespace SpecificSolutions.Endowment.Test.Helper
{
    public class AppDbContextHelper
    {
        private readonly IServiceProvider _provider;

        public AppDbContextHelper(IServiceProvider provider)
        {
            _provider = provider;
        }

        public AppDbContext Context()
        {
            using var scope = _provider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // add connection string here

            appDbContext.Database.GetDbConnection().ConnectionString = "Server=.;Database=Endowment_test;Trusted_Connection=True;MultipleActiveResultSets=true;  trustservercertificate=true;";

            return appDbContext;
        }
    }
}
