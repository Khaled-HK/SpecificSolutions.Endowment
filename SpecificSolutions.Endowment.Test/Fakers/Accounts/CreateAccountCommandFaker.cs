using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Enums.Accounts;

namespace SpecificSolutions.Endowment.Test.Fakers.Accounts
{
    public sealed class CreateAccountCommandFaker : ParameterlessObjectFaker<CreateAccountCommand>
    {
        public CreateAccountCommandFaker()
        {
            RuleFor(x => x.Name, f => GenerateValidName());
            RuleFor(x => x.MotherName, f => GenerateValidName());
            RuleFor(x => x.BirthDate, f => f.Date.Past(30));
            RuleFor(x => x.Gender, f => f.PickRandom<Gender>());
            RuleFor(x => x.Barcode, f => f.Finance.Account());
            RuleFor(x => x.Status, f => f.PickRandom<Status>());
            RuleFor(x => x.LockerFileNumber, f => f.Random.Int(1, 1000));
            RuleFor(x => x.SocialStatus, f => f.PickRandom<SocialStatus>());
            RuleFor(x => x.BookNumber, f => 55);
            RuleFor(x => x.PaperNumber, f => f.Random.Int(0, 1000));
            RuleFor(x => x.RegistrationNumber, f => f.Random.Int(0, 1000));
            RuleFor(x => x.AccountNumber, f => f.Finance.Account());
            RuleFor(x => x.Type, f => f.PickRandom<AccountType>());
            RuleFor(x => x.LookOver, f => f.Random.Bool());
            RuleFor(x => x.Note, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.NID, f => f.Random.Int(1, 999999999));
            RuleFor(x => x.IsActive, f => true);
            RuleFor(x => x.Balance, f => f.Finance.Amount(0, 100000));
            RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
            RuleFor(x => x.Address, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.City, f => f.Address.City());
            RuleFor(x => x.Country, f => f.Address.Country());
            RuleFor(x => x.ContactNumber, f => GenerateLibyanPhoneNumber());
            RuleFor(x => x.Floors, f => f.Random.Int(1, 10));
        }

        private string GenerateValidName()
        {
            var random = new Random();
            var names = new[] { "Ahmed", "Mohammed", "Ali", "Fatima", "Aisha", "Khadija", "Omar", "Hassan", "Hussein", "Zainab" };
            return names[random.Next(names.Length)];
        }

        private string GenerateLibyanPhoneNumber()
        {
            var random = new Random();
            var prefix = random.Next(2) == 0 ? "09" : "02";
            var secondDigit = prefix == "09" ? random.Next(1, 6) : random.Next(1, 10);
            var remainingDigits = random.Next(1000000, 10000000).ToString();
            return $"{prefix}{secondDigit}-{remainingDigits}";
        }
    }
}
