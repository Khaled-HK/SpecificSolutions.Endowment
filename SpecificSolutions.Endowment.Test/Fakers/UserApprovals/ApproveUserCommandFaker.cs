using Bogus;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser;

namespace SpecificSolutions.Endowment.Test.Fakers.UserApprovals
{
    public sealed class ApproveUserCommandFaker : Faker<ApproveUserCommand>
    {
        public ApproveUserCommandFaker()
        {
            CustomInstantiator(f => new ApproveUserCommand(
                "a2d890d8-01d1-494b-9f62-6336b937e6fc",
                f.PickRandom("Admin", "Employee", "Customer")
            ));
        }
    }
}
