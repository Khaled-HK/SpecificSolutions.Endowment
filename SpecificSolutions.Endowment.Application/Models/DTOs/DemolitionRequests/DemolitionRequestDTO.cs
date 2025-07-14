using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests
{
    public class DemolitionRequestDTO
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public string ContractorName { get; set; }
        public decimal EstimatedCost { get; set; }
        public string EstimatedTime { get; set; }
        public decimal EstimatedRebuildingCost { get; set; }
        public string Status { get; set; }
    }

    public class FilterDemolitionRequestDTO : FilterRequestDTO
    {
        public string EstimatedCost { get; set; }
        public string EstimatedTime { get; set; }
        public string DemolitionReason { get; set; }
        public decimal EstimatedRebuildingCost { get; set; }
        public string ContractorName { get; set; }
        public string Reason { get; set; }
    }
}