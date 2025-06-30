using SpecificSolutions.Endowment.Core.Enums.Buildings;

namespace SpecificSolutions.Endowment.Core.Models.Buildings
{
    public interface ICreateBuildingCommand
    {
        string Name { get; set; }
        string FileNumber { get; set; }
        string Definition { get; set; }
        string Classification { get; set; }
        string OfficeId { get; set; }
        string Unit { get; set; }
        string RegionId { get; set; }
        string NearestLandmark { get; set; }
        DateTime ConstructionDate { get; set; }
        DateTime OpeningDate { get; set; }
        string MapLocation { get; set; }
        double TotalLandArea { get; set; }
        double TotalCoveredArea { get; set; }
        int NumberOfFloors { get; set; }
        string ElectricityMeter { get; set; }
        string AlternativeEnergySource { get; set; }
        string WaterSource { get; set; }
        string Sanitation { get; set; }
        string BriefDescription { get; set; }
        string UserId { get; set; }
        bool ServicesSpecialNeeds { get; set; }
        bool SpecialEntranceWomen { get; set; }
        string PicturePath { get; set; }
        string LandDonorName { get; set; }
        string PrayerCapacity { get; set; }
        SourceFunds SourceFunds { get; set; }
        //HashSet<ICreateBuildingDetailCommand> BuildingDetails { get; set; }
    }
}
