using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Application.Contracts.IRepositories
{
    public interface IMosqueRepository : IRepository<Mosque>
    {
        Task<Mosque> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Mosque>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Mosque mosque, CancellationToken cancellationToken);
        Task UpdateAsync(Mosque mosque);
        Task DeleteAsync(Guid id);
        Task<PagedList<MosqueDTO>> GetByFilterAsync(FilterMosqueQuery query, CancellationToken cancellationToken);
    }
}