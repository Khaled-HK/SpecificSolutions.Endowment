using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;
using SpecificSolutions.Endowment.Core.Entities.Requests;

namespace SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests
{
    public class BuildingDetailRequest
    {
        public Guid Id { get; private set; }
        public string RequestDetails { get; private set; }
        public DateTime RequestDate { get; private set; }
        public Guid BuildingDetailId { get; private set; }
        public BuildingDetail BuildingDetail { get; private set; }

        public Guid RequestId { get; private set; }
        public Request Request { get; private set; }


        // Private constructor for EF Core
        private BuildingDetailRequest() { }

        // Factory method for creating a new BuildingDetailRequest
        public static BuildingDetailRequest Create(string requestDetails, DateTime requestDate, Guid buildingDetailId)
        {
            return new BuildingDetailRequest
            {
                RequestDetails = requestDetails,
                RequestDate = requestDate,
                BuildingDetailId = buildingDetailId
            };
        }
    }
}