namespace SpecificSolutions.Endowment.Application.Models.DTOs.Branchs
{
    public class BranchDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public Guid BankId { get; set; } // Foreign key to Bank
    }
}