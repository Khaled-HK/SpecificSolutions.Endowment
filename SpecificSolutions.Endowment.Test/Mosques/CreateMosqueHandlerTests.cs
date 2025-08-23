using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Test.Asserts.Mosques;
using SpecificSolutions.Endowment.Test.Fakers.Mosques;

namespace SpecificSolutions.Endowment.Test.Mosques
{
    public class CreateMosqueHandlerTests : BaseTest
    {
        protected readonly WebApplicationFactory<Program> _factory;
        private readonly CreateMosqueCommandFaker _faker;

        public CreateMosqueHandlerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            _faker = new CreateMosqueCommandFaker();
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("تم الإضافة بنجاح", result.Message);

            var mosque = await Query(m => m.Mosques
                .Include(m => m.Building)
                .FirstOrDefaultAsync());

            Assert.NotNull(mosque);
            MosqueAssert.AssertEquality(mosque, createMosqueCommandFaker);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesBuildingAndMosque()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            var building = await Query(b => b.Buildings.FirstOrDefaultAsync());
            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());

            Assert.NotNull(building);
            Assert.NotNull(mosque);
            Assert.Equal(building.Id, mosque.BuildingId);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsCorrectUserId()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            var building = await Query(b => b.Buildings.FirstOrDefaultAsync());
            
            Assert.NotNull(building);
            Assert.Equal(user_id, building.UserId);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesMosqueWithCorrectProperties()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            var mosque = await Query(m => m.Mosques
                .Include(m => m.Building)
                .FirstOrDefaultAsync());

            Assert.NotNull(mosque);
            Assert.Equal(createMosqueCommandFaker.MosqueDefinition, mosque.MosqueDefinition);
            Assert.Equal(createMosqueCommandFaker.MosqueClassification, mosque.MosqueClassification);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesBuildingWithCorrectProperties()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            var building = await Query(b => b.Buildings.FirstOrDefaultAsync());

            Assert.NotNull(building);
            Assert.Equal(createMosqueCommandFaker.Name, building.Name);
            Assert.Equal(createMosqueCommandFaker.RegionId, building.RegionId);
            Assert.Equal(createMosqueCommandFaker.OfficeId, building.OfficeId);
            Assert.Equal(createMosqueCommandFaker.FileNumber, building.FileNumber);
        }

        [Fact]
        public async Task Handle_ValidCommand_GeneratesUniqueIds()
        {
            // Arrange
            var createMosqueCommandFaker1 = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            var createMosqueCommandFaker2 = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker1, CancellationToken.None);
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker2, CancellationToken.None);

            // Assert
            var mosques = await Query(m => m.Mosques.ToListAsync());
            var buildings = await Query(b => b.Buildings.ToListAsync());

            Assert.Equal(2, mosques.Count);
            Assert.Equal(2, buildings.Count);
            Assert.NotEqual(mosques[0].Id, mosques[1].Id);
            Assert.NotEqual(buildings[0].Id, buildings[1].Id);
        }

        [Fact]
        public async Task Handle_ValidCommand_CompletesTransaction()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            
            // Verify data is persisted
            var mosqueCount = await Query(m => m.Mosques.CountAsync());
            var buildingCount = await Query(b => b.Buildings.CountAsync());
            
            Assert.Equal(1, mosqueCount);
            Assert.Equal(1, buildingCount);
        }

        [Fact]
        public async Task Handle_ValidCommand_LogsSuccess()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            // Note: Logging verification would require additional setup with ILogger mock
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Handle_InvalidUserId_ReturnsFailureResponse(string invalidUserId)
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, invalidUserId)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User context is unavailable", result.Message);
        }

        [Fact]
        public async Task Handle_ValidCommand_WithAllOptionalFields()
        {
            // Arrange
            var createMosqueCommandFaker = _faker
                .RuleFor(x => x.UserId, (string)user_id)
                .RuleFor(x => x.Definition, "مسجد كبير في وسط المدينة")
                .RuleFor(x => x.Classification, "مسجد جامع")
                .RuleFor(x => x.Unit, "وحدة إدارية")
                .RuleFor(x => x.NearestLandmark, "قرب السوق المركزي")
                .RuleFor(x => x.MapLocation, "32.0853,34.7818")
                .RuleFor(x => x.Sanitation, "صرف صحي متطور")
                .RuleFor(x => x.ElectricityMeter, "EL123456789")
                .RuleFor(x => x.AlternativeEnergySource, "طاقة شمسية")
                .RuleFor(x => x.WaterSource, "مياه بلدية")
                .RuleFor(x => x.BriefDescription, "مسجد حديث البناء مع مرافق متطورة")
                .RuleFor(x => x.LandDonorName, "أحمد محمد علي")
                .RuleFor(x => x.PrayerCapacity, "500")
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            
            var mosque = await Query(m => m.Mosques
                .Include(m => m.Building)
                .FirstOrDefaultAsync());

            Assert.NotNull(mosque);
            Assert.Equal(createMosqueCommandFaker.Definition, mosque.Building.Definition);
            Assert.Equal(createMosqueCommandFaker.Classification, mosque.Building.Classification);
            Assert.Equal(createMosqueCommandFaker.Unit, mosque.Building.Unit);
            Assert.Equal(createMosqueCommandFaker.NearestLandmark, mosque.Building.NearestLandmark);
            Assert.Equal(createMosqueCommandFaker.MapLocation, mosque.Building.MapLocation);
            Assert.Equal(createMosqueCommandFaker.Sanitation, mosque.Building.Sanitation);
            Assert.Equal(createMosqueCommandFaker.ElectricityMeter, mosque.Building.ElectricityMeter);
            Assert.Equal(createMosqueCommandFaker.AlternativeEnergySource, mosque.Building.AlternativeEnergySource);
            Assert.Equal(createMosqueCommandFaker.WaterSource, mosque.Building.WaterSource);
            Assert.Equal(createMosqueCommandFaker.BriefDescription, mosque.Building.BriefDescription);
            Assert.Equal(createMosqueCommandFaker.LandDonorName, mosque.Building.LandDonorName);
            Assert.Equal(createMosqueCommandFaker.PrayerCapacity, mosque.Building.PrayerCapacity);
        }
    }
} 