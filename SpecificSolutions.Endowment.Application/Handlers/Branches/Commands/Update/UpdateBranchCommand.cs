using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Update
{
    [Authorize(Permissions = Permission.BranchEdit)]
    public class UpdateBranchCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int BankId { get; set; } // Foreign key to Bank
    }
}