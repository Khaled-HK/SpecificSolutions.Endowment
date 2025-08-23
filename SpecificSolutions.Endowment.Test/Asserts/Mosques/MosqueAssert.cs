using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Test.Asserts.Mosques
{
    public static class MosqueAssert
    {
        public static void AssertEquality(Mosque mosque, CreateMosqueCommand command)
        {
            Assert.NotNull(mosque);
            Assert.NotNull(command);
            Assert.NotNull(mosque.Building);
            
            // Assert Mosque specific properties
            Assert.Equal(command.MosqueDefinition, mosque.MosqueDefinition);
            Assert.Equal(command.MosqueClassification, mosque.MosqueClassification);
            
            // Assert Building properties
            Assert.Equal(command.Name, mosque.Building.Name);
            Assert.Equal(command.RegionId, mosque.Building.RegionId);
            Assert.Equal(command.OfficeId, mosque.Building.OfficeId);
            Assert.Equal(command.FileNumber, mosque.Building.FileNumber);
            Assert.Equal(command.Definition, mosque.Building.Definition);
            Assert.Equal(command.Classification, mosque.Building.Classification);
            Assert.Equal(command.Unit, mosque.Building.Unit);
            Assert.Equal(command.NearestLandmark, mosque.Building.NearestLandmark);
            Assert.Equal(command.MapLocation, mosque.Building.MapLocation);
            Assert.Equal(command.Sanitation, mosque.Building.Sanitation);
            Assert.Equal(command.ElectricityMeter, mosque.Building.ElectricityMeter);
            Assert.Equal(command.AlternativeEnergySource, mosque.Building.AlternativeEnergySource);
            Assert.Equal(command.WaterSource, mosque.Building.WaterSource);
            Assert.Equal(command.BriefDescription, mosque.Building.BriefDescription);
            Assert.Equal(command.PicturePath, mosque.Building.PicturePath);
            Assert.Equal(command.TotalCoveredArea, mosque.Building.TotalCoveredArea);
            Assert.Equal(command.TotalLandArea, mosque.Building.TotalLandArea);
            Assert.Equal(command.ServicesSpecialNeeds, mosque.Building.ServicesSpecialNeeds);
            Assert.Equal(command.SpecialEntranceWomen, mosque.Building.SpecialEntranceWomen);
            Assert.Equal(command.NumberOfFloors, mosque.Building.NumberOfFloors);
            Assert.Equal(command.OpeningDate, mosque.Building.OpeningDate);
            Assert.Equal(command.ConstructionDate, mosque.Building.ConstructionDate);
            Assert.Equal(command.LandDonorName, mosque.Building.LandDonorName);
            Assert.Equal(command.PrayerCapacity, mosque.Building.PrayerCapacity);
            Assert.Equal(command.SourceFunds, mosque.Building.SourceFunds);
            Assert.Equal(command.UserId, mosque.Building.UserId);
        }

        public static void AssertMosqueProperties(Mosque mosque, CreateMosqueCommand command)
        {
            Assert.NotNull(mosque);
            Assert.Equal(command.MosqueDefinition, mosque.MosqueDefinition);
            Assert.Equal(command.MosqueClassification, mosque.MosqueClassification);
            Assert.NotEqual(Guid.Empty, mosque.Id);
            Assert.NotEqual(Guid.Empty, mosque.BuildingId);
        }

        public static void AssertBuildingProperties(Mosque mosque, CreateMosqueCommand command)
        {
            Assert.NotNull(mosque.Building);
            Assert.Equal(command.Name, mosque.Building.Name);
            Assert.Equal(command.RegionId, mosque.Building.RegionId);
            Assert.Equal(command.OfficeId, mosque.Building.OfficeId);
            Assert.Equal(command.FileNumber, mosque.Building.FileNumber);
            Assert.Equal(command.UserId, mosque.Building.UserId);
            Assert.NotEqual(Guid.Empty, mosque.Building.Id);
        }

        public static void AssertRelationship(Mosque mosque)
        {
            Assert.NotNull(mosque);
            Assert.NotNull(mosque.Building);
            Assert.Equal(mosque.BuildingId, mosque.Building.Id);
        }

        public static void AssertAuditProperties(Mosque mosque, string expectedUserId)
        {
            Assert.NotNull(mosque.Building);
            Assert.Equal(expectedUserId, mosque.Building.UserId);
            // Building entity doesn't have CreatedAt property
        }
    }
}