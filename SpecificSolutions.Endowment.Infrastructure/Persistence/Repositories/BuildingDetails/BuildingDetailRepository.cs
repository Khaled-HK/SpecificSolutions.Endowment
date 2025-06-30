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

    public async Task<BuildingDetail> GetByIdAsync(Guid id)
    {
        return await _context.BuildingDetails.FindAsync(id);
    }

    public async Task<IEnumerable<BuildingDetail>> GetAllAsync()
    {
        return await _context.BuildingDetails.ToListAsync();
    }

    public async Task AddAsync(BuildingDetail buildingDetail)
    {
        await _context.BuildingDetails.AddAsync(buildingDetail);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BuildingDetail buildingDetail)
    {
        _context.BuildingDetails.Update(buildingDetail);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var buildingDetail = await _context.BuildingDetails.FindAsync(id);
        if (buildingDetail != null)
        {
            _context.BuildingDetails.Remove(buildingDetail);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PagedList<BuildingDetailDTO>> GetByFilterAsync(FilterBuildingDetailQuery query, CancellationToken cancellationToken)
    {
        var buildingDetailsQuery = _context.BuildingDetails.Where(b => b.BuildingId == query.BuildingId).AsQueryable();

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            buildingDetailsQuery = buildingDetailsQuery.Where(bd =>
                bd.Name.Contains(query.SearchTerm));
        }

        var dtos = buildingDetailsQuery.Select(bd => new BuildingDetailDTO
        {
            Id = bd.Id,
            Name = bd.Name,
            Floors = bd.Floors,
            WithinMosqueArea = bd.WithinMosqueArea
        });

        return await PagedList<BuildingDetailDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
    }
}