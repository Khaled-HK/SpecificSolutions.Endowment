using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.Decisions;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update
{
    public class UpdateDecisionCommand : ICommand, IUpdateDecisionCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
    }
}