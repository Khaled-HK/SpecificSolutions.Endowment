using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update
{
    [Authorize(Permissions = Permission.RegionEdit)]
    public class UpdateRegionCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public Guid CityId { get; set; }
    }
}