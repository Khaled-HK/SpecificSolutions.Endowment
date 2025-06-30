using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Delete
{
    public class DeleteRegionCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}