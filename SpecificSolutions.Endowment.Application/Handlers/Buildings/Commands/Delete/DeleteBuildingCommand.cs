using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Delete
{
    [Authorize(Permissions = Permission.BuildingDelete)]
    public class DeleteBuildingCommand : ICommand
    {
        public Guid Id { get; set; }
    }
} 