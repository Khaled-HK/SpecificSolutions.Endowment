using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;

namespace SpecificSolutions.Endowment.Core.Models.BuildingDetails
{
    public interface ICreateBuildingDetailCommand
    {
        string Name { get; set; }
        string Description { get; set; }
        bool WithinMosqueArea { get; set; }
        int Floors { get; set; }
        BuildingCategory BuildingCategory { get; set; }
        Guid BuildingId { get; set; }
    }
}
