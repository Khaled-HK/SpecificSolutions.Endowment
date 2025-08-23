using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.NeedsRequestDelete)]
    public class DeleteNeedsRequestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}