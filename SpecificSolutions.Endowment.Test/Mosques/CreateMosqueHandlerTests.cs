using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Test.Asserts.Mosques;
using SpecificSolutions.Endowment.Test.Fakers.Mosques;

namespace SpecificSolutions.Endowment.Test.Mosques
{
    public class CreateMosqueHandlerTests : BaseTest
    {
        protected readonly WebApplicationFactory<Program> _factory;

        public CreateMosqueHandlerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var createMosqueCommandFaker = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle(createMosqueCommandFaker, CancellationToken.None);

            // Assert
            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());

            MosqueAssert.AssertEquality(mosque, createMosqueCommandFaker);
        }
    }
} 