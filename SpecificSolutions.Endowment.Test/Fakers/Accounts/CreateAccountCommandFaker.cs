using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;

namespace SpecificSolutions.Endowment.Test.Fakers.Accounts
{
    public sealed class CreateAccountCommandFaker : ParameterlessObjectFaker<CreateAccountCommand>
    {
        public CreateAccountCommandFaker()
        {
            RuleFor(x => x.Name, f => f.Person.FullName);
            RuleFor(x => x.MotherName, f => f.Person.FullName);
            RuleFor(x => x.BirthDate, f => f.Date.Past(30));
            RuleFor(x => x.AccountNumber, f => f.Finance.Account());
            RuleFor(x => x.Barcode, f => f.Finance.Account());
            RuleFor(x => x.Balance, f => f.Finance.Amount(0, 100000));
            RuleFor(x => x.BookNumber, f => 55);
            RuleFor(x => x.City, f => f.Address.City());
            RuleFor(x => x.ContactNumber, f => f.Phone.PhoneNumber());
            RuleFor(x => x.Country, f => f.Address.Country());
            RuleFor(x => x.Floors, f => f.Random.Int(1, 10));
            RuleFor(x => x.Address, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.Note, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
        }
    }
}
