namespace SpecificSolutions.Endowment.Application.Models.DTOs.Buildings
{
    public class BuildingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileNumber { get; set; }
        public string Definition { get; set; }
        public string Classification { get; set; }
        public string Office { get; set; }
        public string Unit { get; set; }
        public string Region { get; set; }
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
    }
}