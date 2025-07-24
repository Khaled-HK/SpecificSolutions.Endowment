using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create
{
    public class CreateCityCommand : ICommand, ICreateCityCommand
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}