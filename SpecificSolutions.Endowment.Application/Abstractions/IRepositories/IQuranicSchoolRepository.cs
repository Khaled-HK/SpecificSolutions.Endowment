using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IQuranicSchoolRepository : IRepository<QuranicSchool>
    {
        Task<QuranicSchool> GetByIdAsync(Guid id);
        Task<IEnumerable<QuranicSchool>> GetAllAsync();
        Task AddAsync(QuranicSchool product);
        Task UpdateAsync(QuranicSchool product);
        Task DeleteAsync(Guid id);
        Task<PagedList<QuranicSchoolDTO>> GetByFilterAsync(FilterQuranicSchoolQuery query, CancellationToken cancellationToken);
    }
}
