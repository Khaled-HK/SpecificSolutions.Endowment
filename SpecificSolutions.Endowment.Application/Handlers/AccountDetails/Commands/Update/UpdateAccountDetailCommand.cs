using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.AccountDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update
{
    public class UpdateAccountDetailCommand : ICommand, IUpdateAccountDetailCommand
    {
        public Guid Id { get; set; }
        public decimal Debtor { get; set; }
        public decimal Creditor { get; set; }
        public string Note { get; set; }
        public OperationType OperationType { get; set; }
        public int OperationNumber { get; set; }
        public decimal Balance { get; set; }
    }
}