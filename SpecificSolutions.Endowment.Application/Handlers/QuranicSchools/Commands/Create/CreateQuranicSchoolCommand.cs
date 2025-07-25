using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Enums.Buildings;
using SpecificSolutions.Endowment.Core.Models.Buildings;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create
{
    public class CreateQuranicSchoolCommand : ICommand, ICreateBuildingCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileNumber { get; set; }
        public string Definition { get; set; }
        public string Classification { get; set; }
        public string OfficeId { get; set; }
        public string Unit { get; set; }
        public string RegionId { get; set; }
        public string NearestLandmark { get; set; }
        public DateTime ConstructionDate { get; set; }
        public DateTime OpeningDate { get; set; }
        public string MapLocation { get; set; }
        public double TotalLandArea { get; set; }
        public double TotalCoveredArea { get; set; }
        public int NumberOfFloors { get; set; }
        public string ElectricityMeter { get; set; }
        public string AlternativeEnergySource { get; set; }
        public string WaterSource { get; set; }
        public string Sanitation { get; set; }
        public string BriefDescription { get; set; }
        public string UserId { get; set; }
        public bool ServicesSpecialNeeds { get; set; }
        public bool SpecialEntranceWomen { get; set; }
        public string PicturePath { get; set; }
        public string LandDonorName { get; set; }
        public string PrayerCapacity { get; set; }
        public SourceFunds SourceFunds { get; set; }
        //public HashSet<ICreateBuildingDetailCommand> BuildingDetails { get; set; }
    }
}