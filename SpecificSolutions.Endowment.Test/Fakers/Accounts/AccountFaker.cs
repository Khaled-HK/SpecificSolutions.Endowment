using SpecificSolutions.Endowment.Core.Entities.Accounts;

namespace SpecificSolutions.Endowment.Test.Fakers.Accounts;

public sealed class AccountFaker : ParameterlessObjectFaker<Account>
{
    public AccountFaker()
    {
        RuleFor(x => x.Id, f => Guid.NewGuid());
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.Balance, f => f.Finance.Amount(0, 100000));
    }

    public static Account Create()
    {
        var faker = new AccountFaker();
        return faker.Generate();
    }
}