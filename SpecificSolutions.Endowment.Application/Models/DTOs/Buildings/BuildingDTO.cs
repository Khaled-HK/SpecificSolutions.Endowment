namespace SpecificSolutions.Endowment.Application.Models.DTOs.Buildings
{
    public class BuildingDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string FileNumber { get; set; }
        public required string Definition { get; set; }
        public required string Classification { get; set; }
        public required string Office { get; set; }
        public required string Unit { get; set; }
        public required string Region { get; set; }
        public required string NearestLandmark { get; set; }
        public DateTime ConstructionDate { get; set; }
        public DateTime OpeningDate { get; set; }
        public required string MapLocation { get; set; }
        public double TotalLandArea { get; set; }
        public double TotalCoveredArea { get; set; }
        public int NumberOfFloors { get; set; }
        public required string ElectricityMeter { get; set; }
        public required string AlternativeEnergySource { get; set; }
        public required string WaterSource { get; set; }
        public required string Sanitation { get; set; }
        public required string BriefDescription { get; set; }
        public required string UserId { get; set; }
        public required string PicturePath { get; set; }
        public required string LandDonorName { get; set; }
        public required string PrayerCapacity { get; set; }
    }
}