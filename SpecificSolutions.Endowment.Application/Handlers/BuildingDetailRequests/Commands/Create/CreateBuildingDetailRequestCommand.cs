using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Create
{
    [Authorize(Permissions = Permission.BuildingDetailRequestAdd)]
    public class CreateBuildingDetailRequestCommand : ICommand
    {
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid Id { get; set; }
    }
}