using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Delete
{
    public class DeleteConstructionRequestCommand : ICommand
    {
        public Guid ConstructionRequestID { get; set; }
    }
}