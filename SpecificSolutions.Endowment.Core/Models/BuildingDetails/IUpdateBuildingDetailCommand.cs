using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;

namespace SpecificSolutions.Endowment.Core.Models.BuildingDetails
{
    public interface IUpdateBuildingDetailCommand
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Guid BuildingId { get; set; }
        public bool WithinMosqueArea { get; set; }
        public int Floors { get; set; }
        public BuildingCategory BuildingCategory { get; set; }
    }
}
