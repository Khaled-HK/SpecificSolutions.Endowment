namespace SpecificSolutions.Endowment.Application.Models.DTOs.Decisions
{
    public class FilterDecisionDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReferenceNumber { get; set; }
    }
}