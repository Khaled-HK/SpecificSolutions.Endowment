namespace SpecificSolutions.Endowment.Application.Models.DTOs.Requests
{
    public class FilterRequestDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Priority { get; set; }
        public string Location { get; set; }
        public string ReferenceNumber { get; set; }
        public List<string> Attachments { get; set; }
        public string RequestStatus { get; set; }
        public string Description { get; set; }
    }
}