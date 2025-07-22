using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.BuildingDetails;

public class BuildingDetailRepository : Repository<BuildingDetail>, IBuildingDetailRepository
{
    private readonly AppDbContext _context;

    public BuildingDetailRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<BuildingDetail> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.BuildingDetails
            .Include(bd => bd.Building)
            .FirstOrDefaultAsync(bd => bd.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<BuildingDetail>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.BuildingDetails
            .Include(bd => bd.Building)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BuildingDetail buildingDetail, CancellationToken cancellationToken)
    {
        await _context.BuildingDetails.AddAsync(buildingDetail, cancellationToken);
        // Remove SaveChangesAsync here as it should be handled by UnitOfWork
    }

    public async Task UpdateAsync(BuildingDetail buildingDetail)
    {
        _context.BuildingDetails.Update(buildingDetail);
        // Remove SaveChangesAsync here as it should be handled by UnitOfWork
    }

    public async Task DeleteAsync(Guid id)
    {
        var buildingDetail = await _context.BuildingDetails.FindAsync(id);
        if (buildingDetail != null)
        {
            _context.BuildingDetails.Remove(buildingDetail);
            // Remove SaveChangesAsync here as it should be handled by UnitOfWork
        }
    }

    public async Task<PagedList<BuildingDetailDTO>> GetByFilterAsync(FilterBuildingDetailQuery query, CancellationToken cancellationToken)
    {
        var buildingDetails = _context.BuildingDetails
            .Include(bd => bd.Building)
            .AsQueryable();

        // Filter by BuildingId if provided
        if (query.BuildingId.HasValue)
            buildingDetails = buildingDetails.Where(bd => bd.BuildingId == query.BuildingId.Value);

        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            buildingDetails = buildingDetails.Where(bd => bd.Name.Contains(query.SearchTerm));

        // Select the relevant fields to return as DTOs
        var buildingDetailDTOs = buildingDetails.Select(bd => new BuildingDetailDTO
        {
            Id = bd.Id,
            Name = bd.Name,
            Floors = bd.Floors,
            WithinMosqueArea = bd.WithinMosqueArea,
            BuildingCategory = bd.BuildingCategory,
            BuildingId = bd.BuildingId
        });

        // Return paged results
        return await PagedList<BuildingDetailDTO>.CreateAsync(buildingDetailDTOs, query.PageNumber, query.PageSize, cancellationToken);
    }
}