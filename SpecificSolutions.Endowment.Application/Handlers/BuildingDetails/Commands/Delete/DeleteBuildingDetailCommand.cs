using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Delete
{
    [Authorize(Permissions = Permission.FacilityDetailDelete)]
    public class DeleteBuildingDetailCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}