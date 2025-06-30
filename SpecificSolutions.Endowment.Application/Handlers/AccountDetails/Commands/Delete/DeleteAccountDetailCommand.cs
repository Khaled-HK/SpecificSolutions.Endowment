using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Delete
{
    public class DeleteAccountDetailCommand : ICommand
    {
        public Guid Id { get; set; }

        public DeleteAccountDetailCommand(Guid id)
        {
            Id = id;
        }
    }
}