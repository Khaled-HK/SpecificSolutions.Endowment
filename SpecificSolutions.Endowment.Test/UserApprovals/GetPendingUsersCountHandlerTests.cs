using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsersCount;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;
using SpecificSolutions.Endowment.Test.Helper;

namespace SpecificSolutions.Endowment.Test.UserApprovals
{
    /// <summary>
    /// اختبارات GetPendingUsersCountHandler - نمط خالد
    /// يتبع نمط xUnit + BaseTest مثل اختبارات المساجد
    /// </summary>
    public class GetPendingUsersCountHandlerTests : BaseTest
    {
        private GetPendingUsersCountHandler _handler;

        public GetPendingUsersCountHandlerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            using var scope = factory.Services.CreateScope();
            _handler = scope.ServiceProvider.GetRequiredService<GetPendingUsersCountHandler>();
        }

        [Fact]
        public async Task Handle_WithNoFilters_ShouldReturnAllPendingUsersCount()
        {
            // Arrange
            await CreatePendingUsers(3);
            await CreateApprovedUsers(2);

            var query = new GetPendingUsersCountQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data);
        }

        [Fact]
        public async Task Handle_WithSearchTerm_ShouldReturnFilteredCount()
        {
            // Arrange
            await CreatePendingUser("Ahmed", "Test", "ahmed.test@gmail.com");
            await CreatePendingUser("Mohammed", "User", "mohammed.user@gmail.com");
            await CreatePendingUser("Ahmed", "Admin", "ahmed.admin@gmail.com");

            var query = new GetPendingUsersCountQuery(searchTerm: "Ahmed");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data); // Ahmed Test + Ahmed Admin
        }

        [Fact]
        public async Task Handle_WithOfficeFilter_ShouldReturnFilteredCount()
        {
            // Arrange
            var officeId1 = Guid.NewGuid();
            var officeId2 = Guid.NewGuid();

            await CreatePendingUser("User1", "Test", "user1@test.com", officeId1);
            await CreatePendingUser("User2", "Test", "user2@test.com", officeId1);
            await CreatePendingUser("User3", "Test", "user3@test.com", officeId2);

            var query = new GetPendingUsersCountQuery(officeId: officeId1.ToString());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data); // User1 + User2
        }

        [Fact]
        public async Task Handle_WithDateFilter_ShouldReturnFilteredCount()
        {
            // Arrange
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);

            await CreatePendingUser("User1", "Test", "user1@test.com", createdDate: yesterday);
            await CreatePendingUser("User2", "Test", "user2@test.com", createdDate: today);
            await CreatePendingUser("User3", "Test", "user3@test.com", createdDate: tomorrow);

            var query = new GetPendingUsersCountQuery(fromDate: today);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data); // User2 + User3
        }

        [Fact]
        public async Task Handle_WithDateRangeFilter_ShouldReturnFilteredCount()
        {
            // Arrange
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var tomorrow = today.AddDays(1);
            var dayAfterTomorrow = today.AddDays(2);

            await CreatePendingUser("User1", "Test", "user1@test.com", createdDate: yesterday);
            await CreatePendingUser("User2", "Test", "user2@test.com", createdDate: today);
            await CreatePendingUser("User3", "Test", "user3@test.com", createdDate: tomorrow);
            await CreatePendingUser("User4", "Test", "user4@test.com", createdDate: dayAfterTomorrow);

            var query = new GetPendingUsersCountQuery(fromDate: today, toDate: tomorrow);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data); // User2 + User3 + User4
        }

        [Fact]
        public async Task Handle_WithCombinedFilters_ShouldReturnFilteredCount()
        {
            // Arrange
            var officeId = Guid.NewGuid();
            await CreatePendingUser("Ahmed", "Test", "ahmed.test@gmail.com", officeId);
            await CreatePendingUser("Ahmed", "Admin", "ahmed.admin@gmail.com", officeId);
            await CreatePendingUser("Mohammed", "User", "mohammed.user@gmail.com", officeId);

            var query = new GetPendingUsersCountQuery(
                searchTerm: "Ahmed",
                officeId: officeId.ToString(),
                fromDate: DateTime.Today.AddDays(-1));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data); // Ahmed Test + Ahmed Admin
        }

        [Fact]
        public async Task Handle_WithNoPendingUsers_ShouldReturnZero()
        {
            // Arrange
            // لا يوجد مستخدمين معلقين

            var query = new GetPendingUsersCountQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task Handle_WithOnlyApprovedUsers_ShouldReturnZero()
        {
            // Arrange
            await CreateApprovedUsers(3);

            var query = new GetPendingUsersCountQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task Handle_WithInvalidOfficeId_ShouldReturnZero()
        {
            // Arrange
            await CreatePendingUsers(3);

            var query = new GetPendingUsersCountQuery(officeId: "invalid-guid");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task Handle_WithFutureDateFilter_ShouldReturnZero()
        {
            // Arrange
            await CreatePendingUsers(3);

            var query = new GetPendingUsersCountQuery(fromDate: DateTime.Today.AddDays(10));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Data);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task Handle_WithEmptySearchTerm_ShouldReturnAllPendingUsers(string searchTerm)
        {
            // Arrange
            await CreatePendingUsers(3);

            var query = new GetPendingUsersCountQuery(searchTerm: searchTerm);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task Handle_WithEmptyOfficeId_ShouldReturnAllPendingUsers(string officeId)
        {
            // Arrange
            await CreatePendingUsers(3);

            var query = new GetPendingUsersCountQuery(officeId: officeId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data);
        }

        #region Helper Methods

        private async Task CreatePendingUsers(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                await CreatePendingUser($"User{i}", "Test", $"user{i}@test.com");
            }
        }

        private async Task CreateApprovedUsers(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                await CreateApprovedUser($"ApprovedUser{i}", "Test", $"approved{i}@test.com");
            }
        }

        private async Task CreatePendingUser(string firstName, string lastName, string email, Guid? officeId = null, DateTime? createdDate = null)
        {
            var user = ApplicationUser.Seed(
                id: Guid.NewGuid().ToString(),
                email: email,
                firstName: firstName,
                lastName: lastName,
                officeId: officeId ?? Guid.NewGuid(),
                userName: email,
                passwordHash: "test_hash",
                emailConfirmed: true,
                isApproved: false,
                approvedAt: null,
                approvedBy: null);

            user.RejectUser(); // جعل المستخدم معلق

            await Query(async context =>
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return true; // Return value for the Query method
            });
        }

        private async Task CreateApprovedUser(string firstName, string lastName, string email)
        {
            var user = ApplicationUser.Seed(
                id: Guid.NewGuid().ToString(),
                email: email,
                firstName: firstName,
                lastName: lastName,
                officeId: Guid.NewGuid(),
                userName: email,
                passwordHash: "test_hash",
                emailConfirmed: true,
                isApproved: false,
                approvedAt: null,
                approvedBy: null);

            user.ApproveUser("test-admin"); // جعل المستخدم معتمد

            await Query(async context =>
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return true; // Return value for the Query method
            });
        }

        #endregion
    }
}
