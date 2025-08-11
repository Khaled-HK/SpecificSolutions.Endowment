using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard;

namespace SpecificSolutions.Endowment.Application.Handlers.Dashboard.Queries.Filter
{
    public sealed class FilterDashboardQuery : IQuery<DashboardSummaryDTO>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        // خصائص مطلوبة من IQuery حتى لو لم تُستخدم هنا
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}


