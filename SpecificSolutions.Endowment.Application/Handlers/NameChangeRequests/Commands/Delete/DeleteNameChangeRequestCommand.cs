using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.NameChangeRequestDelete)]
    public class DeleteNameChangeRequestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}