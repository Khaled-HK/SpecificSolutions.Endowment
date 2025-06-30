using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Delete
{
    public class DeleteDecisionCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteDecisionCommand(Guid id)
        {
            Id = id;
        }
    }
}