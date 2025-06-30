using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter
{
    public class FilterRequestQuery : IRequest<EndowmentResponse<PagedList<FilterRequestDTO>>>
    {
        public string? Title { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public PageSize PageSize { get; set; } = PageSize.Medium;
    }
}