namespace SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails
{
    public class BuildingDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        //public string ContactNumber { get; set; }
        public int Floors { get; set; }
        public bool WithinMosqueArea { get; set; }
    }
}