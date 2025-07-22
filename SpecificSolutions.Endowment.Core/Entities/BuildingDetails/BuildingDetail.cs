using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;
using SpecificSolutions.Endowment.Core.Enums.BuildingDetails;
using SpecificSolutions.Endowment.Core.Models.BuildingDetails;

namespace SpecificSolutions.Endowment.Core.Entities.BuildingDetails
{
    public class BuildingDetail
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool WithinMosqueArea { get; private set; }
        public int Floors { get; private set; }
        public BuildingCategory BuildingCategory { get; private set; }

        // Navigation property
        public Guid BuildingId { get; private set; }
        public Building Building { get; private set; }

        private HashSet<FacilityDetail> _FacilityDetails = new();
        public IReadOnlyCollection<FacilityDetail> FacilityDetails => _FacilityDetails;

        // relationship with BuildingDetailRequests entity (one to many)
        private HashSet<BuildingDetailRequest> _buildingDetailRequests = new();
        public IReadOnlyCollection<BuildingDetailRequest> BuildingDetailRequests => _buildingDetailRequests;

        // Private constructor for EF Core
        private BuildingDetail() { }

        // Factory method for creating a new BuildingDetail
        public static BuildingDetail Create(ICreateBuildingDetailCommand command)
        {
            return new BuildingDetail
            {
                Name = command.Name,
                WithinMosqueArea = command.WithinMosqueArea,
                Floors = command.Floors,
                BuildingCategory = command.BuildingCategory,
                BuildingId = command.BuildingId,
                //_FacilityDetails = command.CreateFacilityDetailCommands.Select(FacilityDetail.Create).ToHashSet()
            };
        }

        public void Update(IUpdateBuildingDetailCommand command)
        {
            Name = command.Name;
            WithinMosqueArea = command.WithinMosqueArea;
            Floors = command.Floors;
            BuildingCategory = command.BuildingCategory;
        }
    }
}