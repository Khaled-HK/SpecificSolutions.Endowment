using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter
{
    public class FilterChangeOfPathRequestQuery : IQuery<PagedList<ChangeOfPathRequestDTO>>
    {
        public string? SearchTerm { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}