using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails
{
    public class AccountDetailDTO
    {
        public Guid Id { get; set; }
        public decimal Debtor { get; set; }
        public decimal Creditor { get; set; }
        public string Note { get; set; }
        public OperationType OperationType { get; set; }
        public int OperationNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 