using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Delete
{
    public class DeleteBuildingCommand : ICommand
    {
        public Guid Id { get; set; }
    }
} 