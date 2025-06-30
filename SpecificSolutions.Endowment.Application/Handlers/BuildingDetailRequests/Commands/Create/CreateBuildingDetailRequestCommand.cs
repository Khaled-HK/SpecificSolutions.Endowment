using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Create
{
    public class CreateBuildingDetailRequestCommand : ICommand
    {
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid Id { get; set; }
    }
}