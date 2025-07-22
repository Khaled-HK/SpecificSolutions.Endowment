namespace SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails
{
    public class FacilityDetailDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
    }
}