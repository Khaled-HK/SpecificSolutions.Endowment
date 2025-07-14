using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<Request> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ReferenceNumberExists(string referenceNumber);
        void Update(Request request);
        Task AddAsync(Request request, CancellationToken cancellationToken);
        Task<PagedList<FilterRequestDTO>> GetByFilterAsync(FilterRequestQuery query, CancellationToken cancellationToken);
    }
}