using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Entities.Regions;
using SpecificSolutions.Endowment.Core.Enums.Buildings;
using SpecificSolutions.Endowment.Core.Models.Buildings;

namespace SpecificSolutions.Endowment.Core.Entities.Buildings;

public class Building
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string FileNumber { get; private set; }
    public string Definition { get; private set; }
    public string Classification { get; private set; }
    public Guid OfficeId { get; private set; }
    public Office Office { get; private set; }
    public string Unit { get; private set; }
    public Guid RegionId { get; private set; }
    public Region Region { get; private set; }
    public string NearestLandmark { get; private set; }
    public DateTime ConstructionDate { get; private set; }
    public DateTime OpeningDate { get; private set; }
    public string MapLocation { get; private set; }
    public double TotalLandArea { get; private set; }
    public double TotalCoveredArea { get; private set; }
    public int NumberOfFloors { get; private set; }
    public string ElectricityMeter { get; private set; }
    public string AlternativeEnergySource { get; private set; }
    public string WaterSource { get; private set; }
    public string Sanitation { get; private set; }
    public string BriefDescription { get; private set; }
    public string LandDonorName { get; private set; }
    public SourceFunds SourceFunds { get; private set; }
    public string PrayerCapacity { get; private set; }
    public string UserId { get; private set; }
    public bool ServicesSpecialNeeds { get; private set; }
    public bool SpecialEntranceWomen { get; private set; }
    public string PicturePath { get; private set; }// have to be in the blob storage or in the file system or in the database or in the cloud or in the server 
    // Navigation property BuildingDetails

    private HashSet<BuildingDetail> _buildingDetails = new();
    public IReadOnlyCollection<BuildingDetail> BuildingDetails => _buildingDetails;

    private Building() { }

    public static Building Create(ICreateBuildingCommand command)
    {
        return new Building
        {
            Name = command.Name,
            FileNumber = command.FileNumber,
            Definition = command.Definition,
            Classification = command.Classification,
            Unit = command.Unit,
            NearestLandmark = command.NearestLandmark,
            ConstructionDate = command.ConstructionDate,
            OpeningDate = command.OpeningDate,
            MapLocation = command.MapLocation,
            TotalLandArea = command.TotalLandArea,
            TotalCoveredArea = command.TotalCoveredArea,
            NumberOfFloors = command.NumberOfFloors,
            ElectricityMeter = command.ElectricityMeter,
            AlternativeEnergySource = command.AlternativeEnergySource,
            WaterSource = command.WaterSource,
            Sanitation = command.Sanitation,
            BriefDescription = command.BriefDescription,
            UserId = command.UserId,
            ServicesSpecialNeeds = command.ServicesSpecialNeeds,
            SpecialEntranceWomen = command.SpecialEntranceWomen,
            PicturePath = command.PicturePath ?? string.Empty,
            LandDonorName = command.LandDonorName,
            PrayerCapacity = command.PrayerCapacity,
            SourceFunds = command.SourceFunds,
            OfficeId = new Guid(command.OfficeId),
            RegionId = new Guid(command.RegionId),
            //OfficeId = new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
            //RegionId = new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C"),
            //_buildingDetails = command.BuildingDetails.Select(BuildingDetail.Create).ToHashSet(),
        };
    }
    // add static method to update the building

    public void Update(IUpdateBuildingCommand command)
    {
        Name = command.Name;
        FileNumber = command.FileNumber;
        Definition = command.Definition;
        Classification = command.Classification;
        Unit = command.Unit;
        NearestLandmark = command.NearestLandmark;
        ConstructionDate = command.ConstructionDate;
        OpeningDate = command.OpeningDate;
        MapLocation = command.MapLocation;
        TotalLandArea = command.TotalLandArea;
        TotalCoveredArea = command.TotalCoveredArea;
        NumberOfFloors = command.NumberOfFloors;
        ElectricityMeter = command.ElectricityMeter;
        AlternativeEnergySource = command.AlternativeEnergySource;
        WaterSource = command.WaterSource;
        Sanitation = command.Sanitation;
        BriefDescription = command.BriefDescription;
        UserId = command.UserId;
        ServicesSpecialNeeds = command.ServicesSpecialNeeds;
        SpecialEntranceWomen = command.SpecialEntranceWomen;
        PicturePath = command.PicturePath ?? string.Empty;
        LandDonorName = command.LandDonorName;
        PrayerCapacity = command.PrayerCapacity;
        SourceFunds = command.SourceFunds;
        OfficeId = new Guid(command.OfficeId);
        RegionId = new Guid(command.RegionId);
        //_buildingDetails = command.BuildingDetails.Select(BuildingDetail.Create).ToHashSet;
    }
}
