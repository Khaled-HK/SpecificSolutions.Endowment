using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Update
{
    [Authorize(Permissions = Permission.BranchEdit)]
    public class UpdateBranchCommand : ICommand, IUpdateBranchCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactNumber { get; set; }
        public Guid BankId { get; set; }
    }
}