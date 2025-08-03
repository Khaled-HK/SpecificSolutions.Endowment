using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IChangeOfPathRequestRepository : IRepository<ChangeOfPathRequest>
    {
        Task<ChangeOfPathRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<ChangeOfPathRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(ChangeOfPathRequest changeOfPathRequest, CancellationToken cancellationToken);
        Task UpdateAsync(ChangeOfPathRequest changeOfPathRequest);
        Task DeleteAsync(Guid id);
        Task<PagedList<ChangeOfPathRequestDTO>> GetByFilterAsync(FilterChangeOfPathRequestQuery query, CancellationToken cancellationToken);
    }
}
