using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Office;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create
{
    [Authorize(Permissions = Permission.OfficeAdd)]
    public class CreateOfficeCommand : ICommand, ICreateOfficeCommand
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RegionId { get; set; }
    }
}