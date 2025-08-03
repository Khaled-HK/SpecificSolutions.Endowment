using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface INameChangeRequestRepository : IRepository<NameChangeRequest>
    {
        Task<NameChangeRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<NameChangeRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(NameChangeRequest nameChangeRequest, CancellationToken cancellationToken);
        Task UpdateAsync(NameChangeRequest nameChangeRequest);
        Task DeleteAsync(Guid id);
        Task<PagedList<NameChangeRequestDTO>> GetByFilterAsync(FilterNameChangeRequestQuery query, CancellationToken cancellationToken);
    }
}
