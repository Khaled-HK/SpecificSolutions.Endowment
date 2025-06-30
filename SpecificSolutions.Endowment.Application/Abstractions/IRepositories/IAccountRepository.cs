using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Accounts;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<PagedList<FilterAccountDTO>> GetByFilterAsync(FilterAccountQuery query, CancellationToken cancellationToken);
        Task<Account?> GetByIdAsync(Guid id);
    }
}