using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Enums.Buildings;
using SpecificSolutions.Endowment.Core.Models.Buildings;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update
{
    [Authorize(Permissions = Permission.BuildingUpdate)]
    public class UpdateBuildingCommand : ICommand, IUpdateBuildingCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FileNumber { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public Guid OfficeId { get; set; }
        public string Unit { get; set; } = string.Empty;
        public Guid RegionId { get; set; }
        public string NearestLandmark { get; set; } = string.Empty;
        public DateTime ConstructionDate { get; set; }
        public DateTime OpeningDate { get; set; }
        public string MapLocation { get; set; } = string.Empty;
        public double TotalLandArea { get; set; }
        public double TotalCoveredArea { get; set; }
        public int NumberOfFloors { get; set; }
        public string ElectricityMeter { get; set; } = string.Empty;
        public string AlternativeEnergySource { get; set; } = string.Empty;
        public string WaterSource { get; set; } = string.Empty;
        public string Sanitation { get; set; } = string.Empty;
        public string BriefDescription { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool ServicesSpecialNeeds { get; set; }
        public bool SpecialEntranceWomen { get; set; }
        public string PicturePath { get; set; } = string.Empty;
        public string LandDonorName { get; set; } = string.Empty;
        public string PrayerCapacity { get; set; } = string.Empty;
        public SourceFunds SourceFunds { get; set; }
        //public HashSet<ICreateBuildingDetailCommand> BuildingDetails { get; set; }
    }
}