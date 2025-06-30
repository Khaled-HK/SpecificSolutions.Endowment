using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Delete
{
    public class DeleteNameChangeRequestCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}