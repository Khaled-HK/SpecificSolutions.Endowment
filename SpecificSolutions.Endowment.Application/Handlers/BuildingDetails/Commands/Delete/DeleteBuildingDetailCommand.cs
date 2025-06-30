using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Delete
{
    public class DeleteBuildingDetailCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}