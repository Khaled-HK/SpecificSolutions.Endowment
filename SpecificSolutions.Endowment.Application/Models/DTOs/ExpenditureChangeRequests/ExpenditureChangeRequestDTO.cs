using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests
{
    public class ExpenditureChangeRequestDTO : FilterRequestDTO
    {
        public string CurrentExpenditure { get; set; }
        public string NewExpenditure { get; set; }
        public string Reason { get; set; }
    }
}