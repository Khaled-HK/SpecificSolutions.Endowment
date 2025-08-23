using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create
{
    [Authorize(Permissions = Permission.CityAdd)]
    public class CreateCityCommand : ICommand, ICreateCityCommand
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}