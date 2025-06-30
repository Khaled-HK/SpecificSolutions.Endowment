using SpecificSolutions.Endowment.Core.Enums.Mosques;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.Mosques
{
    public class MosqueDTO
    {
        public Guid MosqueID { get; set; }
        public string MosqueName { get; set; }
        public string FileNumber { get; set; }
        public MosqueDefinition MosqueDefinition { get; set; }
        public MosqueClassification MosqueClassification { get; set; }
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
    }
}