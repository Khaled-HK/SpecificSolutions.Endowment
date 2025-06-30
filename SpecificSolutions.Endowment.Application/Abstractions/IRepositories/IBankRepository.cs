using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Banks;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IBankRepository
    {
        Task<Bank> GetByIdAsync(Guid Id);
        Task<IEnumerable<Bank>> GetAllAsync();
        Task AddAsync(Bank bank);
        Task UpdateAsync(Bank bank);
        Task DeleteAsync(Guid Id);
        Task<PagedList<BankDTO>> GetByFilterAsync(FilterBankQuery query, CancellationToken cancellationToken);
    }
}