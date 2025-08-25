using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update
{
    [Authorize(Permissions = Permission.CityUpdate)]
    public class UpdateCityCommand : ICommand, IUpdateCityCommand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
    }
}