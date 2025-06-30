using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Delete
{
    public class DeleteBankCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}