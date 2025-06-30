using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Test.Asserts.Accounts;
using SpecificSolutions.Endowment.Test.Fakers.Accounts;

namespace SpecificSolutions.Endowment.Test.Accounts
{
    public class CreateAccountHandlerTests : BaseTest
    {
        //protected readonly Mock<ICurrentUser> _currentUserMock = new();
        protected readonly WebApplicationFactory<Program> _factory;

        public CreateAccountHandlerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            //_factory = factory.WithWebHostBuilder(builder =>
            //{
            //    builder.ConfigureTestServices(services =>
            //    {
            //        services.SetUnitTestsDefaultEnvironment(currentUser: _currentUserMock.Object);
            //    });
            //});

            //_handlerHelper = new HandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var createAccountCommandfaker = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle(createAccountCommandfaker, CancellationToken.None);

            // Assert
            var account = await Query(a => a.Accounts.FirstOrDefaultAsync());

            AccountAssert.AssertEquality(account, createAccountCommandfaker);
        }
    }
}
