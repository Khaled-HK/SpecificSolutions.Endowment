using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface INameChangeRequestRepository : IRepository<NameChangeRequest>
    {
        Task<NameChangeRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<NameChangeRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(NameChangeRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(NameChangeRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<NameChangeRequestDTO>> GetByFilterAsync(FilterNameChangeRequestQuery query, CancellationToken cancellationToken);
    }
}
