using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Buildings
{
    public class BuildingRepository : Repository<Building>, IBuildingRepository
    {
        private readonly AppDbContext _context;

        public BuildingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Building> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Buildings.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Building>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Buildings.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Building building, CancellationToken cancellationToken)
        {
            await _context.Buildings.AddAsync(building, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Building building)
        {
            _context.Buildings.Update(building);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<BuildingDTO>> GetByFilterAsync(FilterBuildingQuery query, CancellationToken cancellationToken)
        {
            var buildingsQuery = _context.Buildings.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                buildingsQuery = buildingsQuery.Where(b =>
                    b.Name.Contains(query.SearchTerm) ||
                    b.Definition.Contains(query.SearchTerm) ||
                    b.BriefDescription.Contains(query.SearchTerm));
            }

            var buildingDTOs = buildingsQuery.Select(b => new BuildingDTO
            {
                Id = b.Id,
                Name = b.Name,
                FileNumber = b.FileNumber,
                Definition = b.Definition,
                Classification = b.Classification,
                Office = b.Office != null ? b.Office.Name : string.Empty,
                Unit = b.Unit,
                Region = b.Region != null ? b.Region.Name : string.Empty,
                NearestLandmark = b.NearestLandmark,
                ConstructionDate = b.ConstructionDate,
                OpeningDate = b.OpeningDate,
                MapLocation = b.MapLocation,
                TotalLandArea = b.TotalLandArea,
                TotalCoveredArea = b.TotalCoveredArea,
                NumberOfFloors = b.NumberOfFloors,
                ElectricityMeter = b.ElectricityMeter,
                AlternativeEnergySource = b.AlternativeEnergySource,
                WaterSource = b.WaterSource,
                Sanitation = b.Sanitation,
                BriefDescription = b.BriefDescription,
                UserId = b.UserId,
                PicturePath = b.PicturePath,
                LandDonorName = b.LandDonorName,
                PrayerCapacity = b.PrayerCapacity
            });

            return await PagedList<BuildingDTO>.CreateAsync(buildingDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
