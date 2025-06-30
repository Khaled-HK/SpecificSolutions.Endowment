using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Buildings
{
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        public BuildingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
