using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;

namespace SpecificSolutions.Endowment.Core.Models.BuildingDetails
{
    public interface ICreateBuildingDetailCommand
    {
        public string Name { get; set; }
        public bool WithinMosqueArea { get; set; }
        public int Floors { get; set; }
        public BuildingCategory BuildingCategory { get; set; }
        Guid BuildingId { get; set; }
        //HashSet<ICreateFacilityDetailCommand> CreateFacilityDetailCommands { get; set; }
    }
}
