using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Enums.Accounts;
using SpecificSolutions.Endowment.Core.Models.Accounts;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create
{
    [Authorize(Permissions = Permission.AccountAdd)]
    public class CreateAccountCommand : ICommand, ICreateAccountCommand, IAuthorizeableRequest
    {
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
        public string? UserId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ContactNumber { get; set; }
        public int Floors { get; set; }

        public Permission Permission => Permission.AccountAdd;

        public IAsyncEnumerable<AccountDetail>? AccountDetails { get; set; }
    }
}