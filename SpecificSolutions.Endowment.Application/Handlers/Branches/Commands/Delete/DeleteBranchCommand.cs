using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Delete
{
    public class DeleteBranchCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}