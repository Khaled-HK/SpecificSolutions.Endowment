using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Update
{
    public class UpdateBuildingDetailRequestCommand : ICommand
    {
        public Guid Id { get; set; }
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }
        public int BuildingDetailId { get; set; }
    }
}