using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Delete
{
    [Authorize(Permissions = Permission.RegionDelete)]
    public class DeleteRegionCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}