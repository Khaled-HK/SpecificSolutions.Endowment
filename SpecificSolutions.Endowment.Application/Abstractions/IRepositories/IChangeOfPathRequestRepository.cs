using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IChangeOfPathRequestRepository : IRepository<ChangeOfPathRequest>
    {
        Task<ChangeOfPathRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<ChangeOfPathRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(ChangeOfPathRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(ChangeOfPathRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<ChangeOfPathRequestDTO>> GetByFilterAsync(FilterChangeOfPathRequestQuery query, CancellationToken cancellationToken);
    }
}
