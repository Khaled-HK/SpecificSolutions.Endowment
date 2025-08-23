using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete
{
    [Authorize(Permissions = Permission.OfficeDelete)]
    public class DeleteOfficeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}