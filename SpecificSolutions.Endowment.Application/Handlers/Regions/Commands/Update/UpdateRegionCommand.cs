using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Regions;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update
{
    [Authorize(Permissions = Permission.RegionUpdate)]
    public class UpdateRegionCommand : ICommand, IUpdateRegionCommand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
        public Guid CityId { get; set; }
    }
}