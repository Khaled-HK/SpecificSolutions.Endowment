using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetDecisions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Decisions;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IDecisionRepository : IRepository<Decision>
    {
        Task<Decision> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Decision>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Decision decision, CancellationToken cancellationToken);
        void Update(Decision decision);
        void Delete(Decision decision);
        Task<PagedList<FilterDecisionDTO>> GetByFilterAsync(FilterDecisionQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuPair>> GetDecisionsAsync(GetDecisionsQuery query, CancellationToken cancellationToken);
    }
}