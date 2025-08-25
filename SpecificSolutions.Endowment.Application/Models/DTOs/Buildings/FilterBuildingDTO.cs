namespace SpecificSolutions.Endowment.Application.Models.DTOs.Buildings
{
    public class FilterBuildingDTO
    {
        public string? Name { get; set; }
        public string? FileNumber { get; set; }
        public string? Classification { get; set; }
        public string? Unit { get; set; }
        public Guid? OfficeId { get; set; }
        public Guid? RegionId { get; set; }
    }
}
