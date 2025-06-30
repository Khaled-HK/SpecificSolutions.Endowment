using SpecificSolutions.Endowment.Core.Entities.Accounts;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.AccountDetails;

namespace SpecificSolutions.Endowment.Core.Entities.AccountDetails;

public class AccountDetail
{
    private AccountDetail() { }

    public Guid Id { get; private set; }
    public decimal Debtor { get; private set; }
    public decimal Creditor { get; private set; }
    public string Note { get; private set; }
    public OperationType OperationType { get; private set; }
    public int OperationNumber { get; private set; }
    public decimal Balance { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; }
    public DateTime CreatedDate { get; private set; }

    // Constructor to enforce invariants
    public AccountDetail(ICreateAccountDetailCommand command)
    {
        Id = command.Id;
        Debtor = command.Debtor;
        Creditor = command.Creditor;
        Note = command.Note;
        CreatedDate = DateTime.UtcNow;
        OperationType = command.OperationType;
        OperationNumber = command.OperationNumber;
        Balance = command.Balance;
    }

    // Method to update the note
    public void UpdateNote(string note)
    {
        Note = note;
    }

    // Method to update the balance
    public void UpdateBalance(decimal balance)
    {
        Balance = balance;
    }

    public void Update(IUpdateAccountDetailCommand command)
    {
        Debtor = command.Debtor;
        Creditor = command.Creditor;
        Note = command.Note;
        OperationType = command.OperationType;
        OperationNumber = command.OperationNumber;
        Balance = command.Balance;
    }
}