using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create
{
    [Authorize(Permissions = Permission.RegionAdd)]
    public class CreateRegionCommand : ICommand
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public Guid CityId { get; set; }
    }
}