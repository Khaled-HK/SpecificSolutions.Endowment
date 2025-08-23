using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Create
{
    [Authorize(Permissions = Permission.BranchAdd)]
    public class CreateBranchCommand : ICommand
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public Guid Id { get; set; } // Foreign key to Bank
    }
}