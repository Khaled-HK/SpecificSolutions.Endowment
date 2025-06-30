using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.Decisions;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create
{
    public class CreateDecisionCommand : ICommand, ICreateDecisionCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReferenceNumber { get; set; }
    }
}