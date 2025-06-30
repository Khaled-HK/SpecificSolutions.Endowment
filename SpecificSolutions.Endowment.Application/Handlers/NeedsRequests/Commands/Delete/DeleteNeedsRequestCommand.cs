using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Delete
{
    public class DeleteNeedsRequestCommand : ICommand
    {
        public Guid NeedsRequestID { get; set; }
    }
}