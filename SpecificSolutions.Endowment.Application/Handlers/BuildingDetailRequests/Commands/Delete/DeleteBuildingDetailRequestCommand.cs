using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.BuildingDetailRequestDelete)]
    public class DeleteBuildingDetailRequestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}