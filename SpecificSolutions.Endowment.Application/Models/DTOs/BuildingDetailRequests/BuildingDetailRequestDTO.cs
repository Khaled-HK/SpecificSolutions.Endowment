namespace SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests
{
    public class BuildingDetailRequestDTO
    {
        public Guid Id { get; set; }
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid BuildingDetailId { get; set; }
    }
}