using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Enums.Accounts;
using SpecificSolutions.Endowment.Core.Models.Accounts;
namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update
{
    public class UpdateAccountCommand : ICommand, IUpdateAccountCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MotherName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Barcode { get; set; }
        public Status Status { get; set; }
        public int LockerFileNumber { get; set; }
        public SocialStatus SocialStatus { get; set; }
        public int BookNumber { get; set; }
        public int PaperNumber { get; set; }
        public int RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public AccountType Type { get; set; }
        public bool LookOver { get; set; }
        public string Note { get; set; } = string.Empty;
        public int NID { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal Balance { get; set; }
    }
}