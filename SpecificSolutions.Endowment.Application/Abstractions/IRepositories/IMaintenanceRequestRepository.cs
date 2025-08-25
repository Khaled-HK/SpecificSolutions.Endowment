using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IMaintenanceRequestRepository : IRepository<MaintenanceRequest>
    {
        Task<MaintenanceRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<MaintenanceRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(MaintenanceRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(MaintenanceRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<MaintenanceRequestDTO>> GetByFilterAsync(FilterMaintenanceRequestQuery query, CancellationToken cancellationToken);
    }
}
