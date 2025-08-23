using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Delete
{
    [Authorize(Permissions = Permission.AccountDelete)]
    public class DeleteAccountCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteAccountCommand(Guid id)
        {
            Id = id;
        }
    }
}