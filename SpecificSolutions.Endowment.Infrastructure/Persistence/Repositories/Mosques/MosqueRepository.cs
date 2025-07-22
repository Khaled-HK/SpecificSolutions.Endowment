using SpecificSolutions.Endowment.Application.Contracts.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Mosques
{
    public class MosqueRepository : Repository<Mosque>, IMosqueRepository
    {
        private readonly AppDbContext _context;

        public MosqueRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedList<MosqueDTO>> GetByFilterAsync(FilterMosqueQuery query, CancellationToken cancellationToken)
        {
            var mosques = _context.Mosques
                .Include(m => m.Building)
                    .ThenInclude(b => b.Office)
                .Include(m => m.Building)
                    .ThenInclude(b => b.Region)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                mosques = mosques.Where(m => m.Building.Name.Contains(query.SearchTerm));

            // Select the relevant fields to return as DTOs
            var accountDTOs = mosques.Select(a => new MosqueDTO
            {
                MosqueID = a.Id,
                BuildingId = a.BuildingId,
                MosqueClassification = a.MosqueClassification,
                MapLocation = a.Building.MapLocation,
                // complete all 
                AlternativeEnergySource = a.Building.AlternativeEnergySource,
                BriefDescription = a.Building.BriefDescription,
                ConstructionDate = a.Building.ConstructionDate,
                ElectricityMeter = a.Building.ElectricityMeter,
                FileNumber = a.Building.FileNumber,
                MosqueDefinition = a.MosqueDefinition,
                MosqueName = a.Building.Name,
                NearestLandmark = a.Building.NearestLandmark,
                NumberOfFloors = a.Building.NumberOfFloors,
                OpeningDate = a.Building.OpeningDate,
                Sanitation = a.Building.Sanitation,
                TotalCoveredArea = a.Building.TotalCoveredArea,
                TotalLandArea = a.Building.TotalLandArea,
                WaterSource = a.Building.WaterSource,
                Unit = a.Building.Unit,
                Office = a.Building.Office != null ? a.Building.Office.Name : string.Empty,
                Region = a.Building.Region != null ? a.Building.Region.Name : string.Empty,
            });

            // Return paged results
            return await PagedList<MosqueDTO>.CreateAsync(accountDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }

        public async Task<Mosque?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Mosques
                .Include(m => m.Building)
                    .ThenInclude(b => b.Office)
                .Include(m => m.Building)
                    .ThenInclude(b => b.Region)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Mosque>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Mosques.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Mosque mosque, CancellationToken cancellationToken)
        {
            await _context.Mosques.AddAsync(mosque, cancellationToken);
            // Remove SaveChangesAsync here as it should be handled by UnitOfWork
        }

        public async Task UpdateAsync(Mosque mosque)
        {
            _context.Mosques.Update(mosque);
            // Remove SaveChangesAsync here as it should be handled by UnitOfWork
        }

        public async Task DeleteAsync(Guid id)
        {
            var mosque = await _context.Mosques.FindAsync(id);
            if (mosque != null)
            {
                _context.Mosques.Remove(mosque);
                // Remove SaveChangesAsync here as it should be handled by UnitOfWork
            }
        }
    }
}