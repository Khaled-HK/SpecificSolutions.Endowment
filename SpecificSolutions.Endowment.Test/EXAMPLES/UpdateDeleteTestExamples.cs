using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosques;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Test.Fakers.Mosques;
using System.Collections.Generic;

namespace SpecificSolutions.Endowment.Test.EXAMPLES
{
    /// <summary>
    /// أمثلة على اختبارات Update و Delete - مرجع للاختبارات
    /// </summary>

    #region Update Handler Tests Example

    public class UpdateMosqueHandlerTestsExample : BaseTest
    {
        protected readonly WebApplicationFactory<Program> _factory;

        public UpdateMosqueHandlerTestsExample(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Handle_ValidUpdateCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var updateCommand = new UpdateMosqueCommandFaker()
                .RuleFor(x => x.Id, f => mosque.Id)
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<UpdateMosqueCommand, EndowmentResponse>(updateCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("تم التحديث بنجاح", result.Message);
        }

        [Fact]
        public async Task Handle_UpdateCommand_UpdatesEntityCorrectly()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var updateCommand = new UpdateMosqueCommandFaker()
                .RuleFor(x => x.Id, f => mosque.Id)
                .RuleFor(x => x.Name, f => "Updated Mosque Name")
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<UpdateMosqueCommand, EndowmentResponse>(updateCommand, CancellationToken.None);

            // Assert
            var updatedMosque = await Query(m => m.Mosques
                .Include(m => m.Building)
                .FirstOrDefaultAsync(m => m.Id == mosque.Id));

            Assert.NotNull(updatedMosque);
            Assert.Equal("Updated Mosque Name", updatedMosque.Building.Name);
        }

        [Fact]
        public async Task Handle_NonExistentEntity_ReturnsFailureResponse()
        {
            // Arrange
            var updateCommand = new UpdateMosqueCommandFaker()
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<UpdateMosqueCommand, EndowmentResponse>(updateCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("غير موجود", result.Message);
        }

        [Fact]
        public async Task Handle_UpdateCommand_UpdatesOnlySpecifiedFields()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .RuleFor(x => x.Name, f => "Original Name")
                .RuleFor(x => x.Definition, f => "Original Definition")
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var updateCommand = new UpdateMosqueCommandFaker()
                .RuleFor(x => x.Id, f => mosque.Id)
                .RuleFor(x => x.Name, f => "Updated Name")
                .RuleFor(x => x.Definition, f => "Original Definition") // نفس القيمة
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            // Act
            await _handlerHelper.Handle<UpdateMosqueCommand, EndowmentResponse>(updateCommand, CancellationToken.None);

            // Assert
            var updatedMosque = await Query(m => m.Mosques
                .Include(m => m.Building)
                .FirstOrDefaultAsync(m => m.Id == mosque.Id));

            Assert.NotNull(updatedMosque);
            Assert.Equal("Updated Name", updatedMosque.Building.Name);
            Assert.Equal("Original Definition", updatedMosque.Building.Definition);
        }

        [Fact]
        public async Task Handle_WithNullUserId_ReturnsFailureResponse()
        {
            // Arrange - Example of proper null handling
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)null!) // Use null! for non-nullable string
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User context is unavailable", result.Message);
        }

        [Fact]
        public async Task Handle_WithEmptyUserId_ReturnsFailureResponse()
        {
            // Arrange - Example of empty string handling
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => string.Empty) // Empty string
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User context is unavailable", result.Message);
        }

        [Fact]
        public async Task Handle_WithWhitespaceUserId_ReturnsFailureResponse()
        {
            // Arrange - Example of whitespace handling
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => "   ") // Whitespace only
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User context is unavailable", result.Message);
        }
    }

    #endregion

    #region Delete Handler Tests Example

    public class DeleteMosqueHandlerTestsExample : BaseTest
    {
        protected readonly WebApplicationFactory<Program> _factory;

        public DeleteMosqueHandlerTestsExample(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Handle_ValidDeleteCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var deleteCommand = new DeleteMosqueCommand { Id = mosque.Id };

            // Act
            var result = await _handlerHelper.Handle<DeleteMosqueCommand, EndowmentResponse>(deleteCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("تم الحذف بنجاح", result.Message);
        }

        [Fact]
        public async Task Handle_ValidDeleteCommand_RemovesEntityFromDatabase()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var deleteCommand = new DeleteMosqueCommand { Id = mosque.Id };

            // Act
            await _handlerHelper.Handle<DeleteMosqueCommand, EndowmentResponse>(deleteCommand, CancellationToken.None);

            // Assert
            var deletedMosque = await Query(m => m.Mosques.FirstOrDefaultAsync(m => m.Id == mosque.Id));
            Assert.Null(deletedMosque);
        }

        [Fact]
        public async Task Handle_DeleteCommand_RemovesRelatedEntities()
        {
            // Arrange
            var createCommand = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand, CancellationToken.None);

            var mosque = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var deleteCommand = new DeleteMosqueCommand { Id = mosque.Id };

            // Act
            await _handlerHelper.Handle<DeleteMosqueCommand, EndowmentResponse>(deleteCommand, CancellationToken.None);

            // Assert
            var deletedMosque = await Query(m => m.Mosques.FirstOrDefaultAsync(m => m.Id == mosque.Id));
            var deletedBuilding = await Query(b => b.Buildings.FirstOrDefaultAsync(b => b.Id == mosque.BuildingId));

            Assert.Null(deletedMosque);
            Assert.Null(deletedBuilding);
        }

        [Fact]
        public async Task Handle_NonExistentEntity_ReturnsFailureResponse()
        {
            // Arrange
            var deleteCommand = new DeleteMosqueCommand { Id = Guid.NewGuid() };

            // Act
            var result = await _handlerHelper.Handle<DeleteMosqueCommand, EndowmentResponse>(deleteCommand, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("غير موجود", result.Message);
        }

        [Fact]
        public async Task Handle_DeleteCommand_DoesNotAffectOtherEntities()
        {
            // Arrange
            var createCommand1 = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            var createCommand2 = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand1, CancellationToken.None);
            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(createCommand2, CancellationToken.None);

            var mosque1 = await Query(m => m.Mosques.FirstOrDefaultAsync());
            var mosque2 = await Query(m => m.Mosques.Skip(1).FirstOrDefaultAsync());

            var deleteCommand = new DeleteMosqueCommand { Id = mosque1.Id };

            // Act
            await _handlerHelper.Handle<DeleteMosqueCommand, EndowmentResponse>(deleteCommand, CancellationToken.None);

            // Assert
            var deletedMosque = await Query(m => m.Mosques.FirstOrDefaultAsync(m => m.Id == mosque1.Id));
            var remainingMosque = await Query(m => m.Mosques.FirstOrDefaultAsync(m => m.Id == mosque2.Id));

            Assert.Null(deletedMosque);
            Assert.NotNull(remainingMosque);
        }
    }

    #endregion

    #region Query Handler Tests Example

    public class GetMosquesHandlerTestsExample : BaseTest
    {
        protected readonly WebApplicationFactory<Program> _factory;

        public GetMosquesHandlerTestsExample(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Handle_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            await CreateMosques(10);

            var query = new GetMosquesQuery { PageNumber = 2, PageSize = 3 };

            // Act
            var result = await _handlerHelper.Handle<GetMosquesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count());
        }

        [Fact]
        public async Task Handle_WithFilter_ReturnsFilteredResults()
        {
            // Arrange
            await CreateMosques(5);
            await CreateMosqueWithName("Special Mosque");

            var query = new GetMosquesQuery 
            { 
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _handlerHelper.Handle<GetMosquesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(6, result.Data.Count());
            Assert.Contains(result.Data, kvp => kvp.Value == "Special Mosque");
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetMosquesQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handlerHelper.Handle<GetMosquesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        #region Helper Methods

        private async Task CreateMosques(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var command = new CreateMosqueCommandFaker()
                    .RuleFor(x => x.UserId, f => (string)user_id)
                    .RuleFor(x => x.Name, f => $"Mosque {i}")
                    .Generate();

                await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(command, CancellationToken.None);
            }
        }

        private async Task CreateMosqueWithName(string name)
        {
            var command = new CreateMosqueCommandFaker()
                .RuleFor(x => x.UserId, f => (string)user_id)
                .RuleFor(x => x.Name, f => name)
                .Generate();

            await _handlerHelper.Handle<CreateMosqueCommand, EndowmentResponse>(command, CancellationToken.None);
        }

        #endregion
    }

    #endregion
}
