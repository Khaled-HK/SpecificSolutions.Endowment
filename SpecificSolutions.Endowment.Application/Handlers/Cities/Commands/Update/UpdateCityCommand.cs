using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update
{
    [Authorize(Permissions = Permission.CityEdit)]
    public class UpdateCityCommand : ICommand, IUpdateCityCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}