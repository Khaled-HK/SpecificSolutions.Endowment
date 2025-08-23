using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete
{
    [Authorize(Permissions = Permission.FacilityDetailDelete)]
    public class DeleteFacilityDetailCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}