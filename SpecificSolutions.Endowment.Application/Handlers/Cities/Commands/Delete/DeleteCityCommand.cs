using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete
{
    public class DeleteCityCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}