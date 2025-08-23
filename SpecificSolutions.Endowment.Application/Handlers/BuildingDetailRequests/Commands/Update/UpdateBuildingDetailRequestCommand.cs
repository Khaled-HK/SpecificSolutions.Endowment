using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Update
{
    [Authorize(Permissions = Permission.BuildingDetailRequestEdit)]
    public class UpdateBuildingDetailRequestCommand : ICommand
    {
        public Guid Id { get; set; }
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }
        public int BuildingDetailId { get; set; }
    }
}