using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Delete
{
    [Authorize(Permissions = Permission.DecisionDelete)]
    public class DeleteDecisionCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteDecisionCommand(Guid id)
        {
            Id = id;
        }
    }
}