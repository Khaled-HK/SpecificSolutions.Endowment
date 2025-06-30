using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Delete
{
    public class DeleteBuildingDetailRequestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}