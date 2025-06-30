using SpecificSolutions.Endowment.Core.Entities.AccountDetails;
using SpecificSolutions.Endowment.Core.Enums.Accounts;
using SpecificSolutions.Endowment.Core.Models.Accounts;

namespace SpecificSolutions.Endowment.Core.Entities.Accounts;

public sealed class Account
{
    private Account() { }

    //todo:NewGuid
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string MotherName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public string Barcode { get; private set; }
    public Status Status { get; private set; }
    public int LockerFileNumber { get; private set; }
    public SocialStatus SocialStatus { get; private set; }
    public int BookNumber { get; private set; }
    public int PaperNumber { get; private set; }
    public int RegistrationNumber { get; private set; }
    public string AccountNumber { get; private set; }
    public AccountType Type { get; private set; }
    public bool LookOver { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public int NID { get; private set; }
    public bool IsActive { get; private set; } = true;
    public decimal Balance { get; private set; }
    public string UserId { get; private set; }

    private HashSet<AccountDetail> _accountDetails = new();
    public IReadOnlyCollection<AccountDetail> AccountDetails => _accountDetails;

    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ContactNumber { get; set; }
    public int Floors { get; set; }

    public static Account Create(ICreateAccountCommand command, string userId)
        => new Account(command);

    public void Apply(ICreateAccountCommand @event)
    {
    }

    private Account(ICreateAccountCommand command)
    {
        Name = command.Name;
        MotherName = command.MotherName;
        BirthDate = command.BirthDate;
        Gender = command.Gender;
        Barcode = command.Barcode;
        Status = command.Status;
        LockerFileNumber = command.LockerFileNumber;
        SocialStatus = command.SocialStatus;
        BookNumber = command.BookNumber;
        PaperNumber = command.PaperNumber;
        RegistrationNumber = command.RegistrationNumber;
        AccountNumber = command.AccountNumber;
        Type = command.Type;
        LookOver = command.LookOver;
        Note = command.Note;
        NID = command.NID;
        IsActive = command.IsActive;
        Balance = command.Balance;
        UserId = command.UserId;
        Address = command.Address;
        City = command.City;
        Country = command.Country;
        ContactNumber = command.ContactNumber;
        Floors = command.Floors;

    }

    public void Update(IUpdateAccountCommand command)
    {
        Name = command.Name;
        MotherName = command.MotherName;
        BirthDate = command.BirthDate;
        Gender = command.Gender;
        Barcode = command.Barcode;
        Status = command.Status;
        LockerFileNumber = command.LockerFileNumber;
        SocialStatus = command.SocialStatus;
        BookNumber = command.BookNumber;
        PaperNumber = command.PaperNumber;
        RegistrationNumber = command.RegistrationNumber;
        AccountNumber = command.AccountNumber;
        Type = command.Type;
        LookOver = command.LookOver;
        Note = command.Note;
        NID = command.NID;
        IsActive = command.IsActive;
        Balance = command.Balance;
    }

    public void AddAccountDetail(AccountDetail accountDetail)
    {
        _accountDetails.Add(accountDetail);
    }
}
