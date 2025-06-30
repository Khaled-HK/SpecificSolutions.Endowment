using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter
{
    public class FilterDecisionQuery : IQuery<PagedList<FilterDecisionDTO>>
    {
        public string Title { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}