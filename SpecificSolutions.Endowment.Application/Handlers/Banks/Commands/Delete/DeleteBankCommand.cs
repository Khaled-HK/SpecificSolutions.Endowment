using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Delete
{
    [Authorize(Permissions = Permission.BankDelete)]
    public class DeleteBankCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}