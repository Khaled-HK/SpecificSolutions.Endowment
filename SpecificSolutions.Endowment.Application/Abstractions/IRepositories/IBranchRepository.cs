using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Branchs;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<Branch> GetByIdAsync(Guid Id);
        Task<IEnumerable<Branch>> GetAllAsync();
        Task AddAsync(Branch branch);
        Task UpdateAsync(Branch branch);
        Task DeleteAsync(Guid Id);
        Task<PagedList<BranchDTO>> GetByFilterAsync(FilterBranchQuery query, CancellationToken cancellationToken);
    }
}