namespace SpecificSolutions.Endowment.Application.Models.DTOs.Regions
{
    public class FilterRegionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public Guid CityId { get; set; }
    }
}