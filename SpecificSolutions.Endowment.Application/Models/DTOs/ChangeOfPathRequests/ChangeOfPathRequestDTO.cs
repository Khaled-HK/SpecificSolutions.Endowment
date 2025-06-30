using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests
{
    public class ChangeOfPathRequestDTO : FilterRequestDTO
    {
        public string CurrentType { get; set; }
        public string NewType { get; set; }
        public string Reason { get; set; }
    }
}