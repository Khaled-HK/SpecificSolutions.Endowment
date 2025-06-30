using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Core.Models.AccountDetails
{
    public interface IUpdateAccountDetailCommand
    {
        Guid Id { get; set; }
        decimal Debtor { get; set; }
        decimal Creditor { get; set; }
        string Note { get; set; }
        OperationType OperationType { get; set; }
        int OperationNumber { get; set; }
        decimal Balance { get; set; }
    }
}
