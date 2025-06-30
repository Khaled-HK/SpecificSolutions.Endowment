using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Delete
{
    public class DeleteDemolitionRequestCommand : ICommand
    {
        public Guid DemolitionRequestID { get; set; }
    }
}