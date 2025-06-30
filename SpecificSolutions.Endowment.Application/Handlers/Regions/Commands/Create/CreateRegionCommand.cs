using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create
{
    public class CreateRegionCommand : ICommand
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}