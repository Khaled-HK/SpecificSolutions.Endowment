using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests
{
    public class NameChangeRequestDTO : FilterRequestDTO
    {
        public string CurrentName { get; set; }
        public string NewName { get; set; }
        public string Reason { get; set; }
    }
}