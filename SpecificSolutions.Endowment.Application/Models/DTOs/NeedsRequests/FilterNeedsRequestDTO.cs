using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests
{
    public class FilterNeedsRequestDTO : FilterRequestDTO
    {
        public string NeedsType { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Provider { get; set; }
    }
}