using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Update
{
    [Authorize(Permissions = Permission.BankUpdate)]
    public class UpdateBankCommand : ICommand, IUpdateBankCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}