namespace SpecificSolutions.Endowment.Application.Models.DTOs.Facilities
{
    public class FacilityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}