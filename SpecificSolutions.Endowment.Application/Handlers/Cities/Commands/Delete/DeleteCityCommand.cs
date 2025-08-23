using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete
{
    [Authorize(Permissions = Permission.CityDelete)]
    public class DeleteCityCommand : ICommand, IDeleteCityCommand
    {
        public Guid Id { get; set; }
    }
}