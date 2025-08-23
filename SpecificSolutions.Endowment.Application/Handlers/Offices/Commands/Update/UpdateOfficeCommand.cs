using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Office;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update
{
    [Authorize(Permissions = Permission.OfficeEdit)]
    public class UpdateOfficeCommand : ICommand, IUpdateOfficeCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
    }
}