using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Application.Contracts.IRepositories
{
    public interface IMosqueRepository : IRepository<Mosque>
    {
        Task<Mosque> GetByIdAsync(Guid id);
        Task<IEnumerable<Mosque>> GetAllAsync();
        Task AddAsync(Mosque mosque);
        Task UpdateAsync(Mosque mosque);
        Task DeleteAsync(Guid id);
        Task<PagedList<MosqueDTO>> GetByFilterAsync(FilterMosqueQuery query, CancellationToken cancellationToken);
    }
}