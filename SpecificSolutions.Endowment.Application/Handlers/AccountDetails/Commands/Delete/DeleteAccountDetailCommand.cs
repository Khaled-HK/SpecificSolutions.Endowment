using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Delete
{
    [Authorize(Permissions = Permission.AccountDetailDelete)]
    public class DeleteAccountDetailCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteAccountDetailCommand(Guid id)
        {
            Id = id;
        }
    }
}