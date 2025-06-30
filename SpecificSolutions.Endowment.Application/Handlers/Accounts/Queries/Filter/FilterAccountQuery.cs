using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Enums.Accounts;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter
{
    public class FilterAccountQuery : IQuery<PagedList<FilterAccountDTO>>
    {
        public string? Name { get; set; }
        public string? MotherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}