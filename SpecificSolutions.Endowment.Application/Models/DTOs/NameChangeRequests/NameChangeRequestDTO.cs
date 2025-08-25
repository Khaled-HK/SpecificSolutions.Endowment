using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests
{
    public class NameChangeRequestDTO : FilterRequestDTO
    {
        public string CurrentName { get; set; } = string.Empty;
        public string NewName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string BuildingType { get; set; } = string.Empty;
    }
}