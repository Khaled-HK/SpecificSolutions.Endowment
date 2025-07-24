using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete
{
    public class DeleteCityCommand : ICommand, IDeleteCityCommand
    {
        public Guid Id { get; set; }
    }
}