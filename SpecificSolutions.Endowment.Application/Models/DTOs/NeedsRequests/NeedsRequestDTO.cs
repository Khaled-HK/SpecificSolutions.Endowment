using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests
{
    public class NeedsRequestDTO
    {
        public Guid Id { get; set; }
        public string NeedsType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double EstimatedCost { get; set; }
        public string Provider { get; set; } = string.Empty;
        public Guid RequestId { get; set; }
    }
} 