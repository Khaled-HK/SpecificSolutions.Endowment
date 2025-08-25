namespace SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests
{
    public class FilterExpenditureChangeRequestDTO
    {
        public string? CurrentExpenditureBranch { get; set; }
        public string? NewExpenditureBranch { get; set; }
        public string? Reason { get; set; }
        public Guid? RequestId { get; set; }
    }
}
