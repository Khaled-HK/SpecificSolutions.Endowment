using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IQuranicSchoolRepository : IRepository<QuranicSchool>
    {
        Task<QuranicSchool> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<QuranicSchool>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(QuranicSchool quranicSchool, CancellationToken cancellationToken);
        Task UpdateAsync(QuranicSchool quranicSchool);
        Task DeleteAsync(Guid id);
        Task<PagedList<QuranicSchoolDTO>> GetByFilterAsync(FilterQuranicSchoolQuery query, CancellationToken cancellationToken);
    }
}
