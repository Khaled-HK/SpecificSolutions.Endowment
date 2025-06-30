using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests
{
    public class MaintenanceRequestDTO : FilterRequestDTO
    {
        public string MaintenanceType { get; set; }
        public double EstimatedCost { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }
}