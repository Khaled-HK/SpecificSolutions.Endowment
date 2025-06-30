using SpecificSolutions.Endowment.Core.Enums.Accounts;

namespace SpecificSolutions.Endowment.Core.Models.Accounts
{
    public interface IUpdateAccountCommand
    {
        string Name { get; set; }
        string MotherName { get; set; }
        DateTime BirthDate { get; set; }
        Gender Gender { get; set; }
        string Barcode { get; set; }
        Status Status { get; set; }
        int LockerFileNumber { get; set; }
        SocialStatus SocialStatus { get; set; }
        int BookNumber { get; set; }
        int PaperNumber { get; set; }
        int RegistrationNumber { get; set; }
        string AccountNumber { get; set; }
        AccountType Type { get; set; }
        bool LookOver { get; set; }
        string Note { get; set; }
        int NID { get; set; }
        bool IsActive { get; set; }
        decimal Balance { get; set; }
    }
}
