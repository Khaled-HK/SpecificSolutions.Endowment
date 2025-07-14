using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails
{
    public class BuildingDetailDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool WithinMosqueArea { get; set; }
        public int Floors { get; set; }
        public BuildingCategory BuildingCategory { get; set; }
        public Guid BuildingId { get; set; }
    }
}