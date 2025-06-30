using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Create
{
    public class CreateBranchCommand : ICommand
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public Guid Id { get; set; } // Foreign key to Bank
    }
}