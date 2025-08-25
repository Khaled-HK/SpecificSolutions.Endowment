using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Office;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update
{
    [Authorize(Permissions = Permission.OfficeUpdate)]
    public class UpdateOfficeCommand : ICommand, IUpdateOfficeCommand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid RegionId { get; set; }
    }
}