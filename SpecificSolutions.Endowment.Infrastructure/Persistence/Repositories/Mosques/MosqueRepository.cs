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
            var mosques = _context.Mosques.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                mosques = mosques.Where(m => m.Building.Name == query.SearchTerm);

            // Select the relevant fields to return as DTOs
            var accountDTOs = mosques.Select(a => new MosqueDTO
            {
                MosqueID = a.Building.Id,
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
                Office = a.Building.Office.Name,
                Region = a.Building.Region.Name,

            });

            // Return paged results
            return await PagedList<MosqueDTO>.CreateAsync(accountDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }

        public async Task<Mosque> GetByIdAsync(Guid id)
        {
            return await _context.Mosques.FindAsync(id);
        }

        public async Task<IEnumerable<Mosque>> GetAllAsync()
        {
            return await _context.Mosques.ToListAsync();
        }

        public async Task AddAsync(Mosque mosque)
        {
            await _context.Mosques.AddAsync(mosque);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mosque mosque)
        {
            _context.Mosques.Update(mosque);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var mosque = await _context.Mosques.FindAsync(id);
            if (mosque != null)
            {
                _context.Mosques.Remove(mosque);
                await _context.SaveChangesAsync();
            }
        }
    }
}