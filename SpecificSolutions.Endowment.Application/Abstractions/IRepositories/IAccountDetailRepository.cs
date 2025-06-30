using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IAccountDetailRepository : IRepository<AccountDetail>
    {
        Task<AccountDetail> GetByIdAsync(Guid id);
        Task AddAsync(AccountDetail accountDetail);
        Task UpdateAsync(AccountDetail accountDetail);
        Task RemoveAsync(AccountDetail accountDetail);
        Task<bool> ExistsAsync(Guid id);
        Task<AccountDetail?> GetByAccountDetailIdAsync(Guid accountDetailId);
        Task<PagedList<FilterAccountDetailDTO>> GetByFilterAsync(FilterAccountDetailQuery query, CancellationToken cancellationToken);
    }
}