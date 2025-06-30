using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Delete
{
    public class DeleteChangeOfPathRequestCommand : ICommand
    {
        public Guid ChangeOfPathRequestID { get; set; }
    }
}