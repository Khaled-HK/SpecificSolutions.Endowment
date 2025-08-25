using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<Request> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<Request>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(Request request, CancellationToken cancellationToken);
        Task UpdateAsync(Request request);
        void Update(Request request);
        Task DeleteAsync(Guid id);
        Task<PagedList<FilterRequestDTO>> GetByFilterAsync(FilterRequestQuery query, CancellationToken cancellationToken);
        Task<bool> ReferenceNumberExists(string referenceNumber);
    }
}