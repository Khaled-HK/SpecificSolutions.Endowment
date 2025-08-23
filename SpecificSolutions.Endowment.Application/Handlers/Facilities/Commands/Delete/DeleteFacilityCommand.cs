using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Delete
{
    [Authorize(Permissions = Permission.FacilityDelete)]
    public class DeleteFacilityCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}