using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;

namespace SpecificSolutions.Endowment.Core.Models.BuildingDetails
{
    public interface IUpdateBuildingDetailCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool WithinMosqueArea { get; set; }
        public int Floors { get; set; }
        public BuildingCategory BuildingCategory { get; set; }
    }
}
