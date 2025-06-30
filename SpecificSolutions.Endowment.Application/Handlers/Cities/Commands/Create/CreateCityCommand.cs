using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create
{
    public class CreateCityCommand : ICommand
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}