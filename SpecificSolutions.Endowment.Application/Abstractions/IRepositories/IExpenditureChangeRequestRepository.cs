using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ExpenditureChangeRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IExpenditureChangeRequestRepository : IRepository<ExpenditureChangeRequest>
    {
        Task<ExpenditureChangeRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<ExpenditureChangeRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(ExpenditureChangeRequest expenditureChangeRequest, CancellationToken cancellationToken);
        Task UpdateAsync(ExpenditureChangeRequest expenditureChangeRequest);
        Task DeleteAsync(Guid id);
        Task<PagedList<ExpenditureChangeRequestDTO>> GetByFilterAsync(FilterExpenditureChangeRequestQuery query, CancellationToken cancellationToken);
    }
}
