using SpecificSolutions.Endowment.Application.Handlers.Dashboard.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDTO> GetByFilterAsync(FilterDashboardQuery query, CancellationToken cancellationToken);
    }
}


