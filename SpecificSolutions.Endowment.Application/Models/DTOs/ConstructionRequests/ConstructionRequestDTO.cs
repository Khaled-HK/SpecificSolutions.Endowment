using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests
{
    public class ConstructionRequestDTO : FilterRequestDTO
    {
        public string BuildingType { get; set; }
        public string ProposedLocation { get; set; }
        public double ProposedArea { get; set; }
        public double EstimatedCost { get; set; }
        public string ContractorName { get; set; }
    }
}