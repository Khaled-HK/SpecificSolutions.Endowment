using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsers;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Test.UserApprovals
{
    /// <summary>
    /// اختبارات GetPendingUsersHandler - نمط خالد
    /// يتبع نمط xUnit + BaseTest مثل اختبارات المساجد
    /// </summary>
    public class GetPendingUsersHandlerTests : BaseTest
    {
        private GetPendingUsersHandler _handler;

        public GetPendingUsersHandlerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            using var scope = factory.Services.CreateScope();
            _handler = scope.ServiceProvider.GetRequiredService<GetPendingUsersHandler>();
        }

        [Fact]
        public async Task Handle_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            await CreatePendingUsers(10); // إنشاء 10 مستخدمين معلقين

            var query = new GetPendingUsersQuery(pageNumber: 2, pageSize: 3);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count); // الصفحة الثانية تحتوي على 3 مستخدمين
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyList()
        {
            // Arrange
            // لا يوجد مستخدمين معلقين

            var query = new GetPendingUsersQuery(pageNumber: 1, pageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WithLargePageSize_ShouldReturnAllUsers()
        {
            // Arrange
            await CreatePendingUsers(5);

            var query = new GetPendingUsersQuery(pageNumber: 1, pageSize: 100);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(5, result.Data.Count);
        }

        [Fact]
        public async Task Handle_WithInvalidPageNumber_ShouldReturnEmptyList()
        {
            // Arrange
            await CreatePendingUsers(5);

            var query = new GetPendingUsersQuery(pageNumber: 10, pageSize: 10); // صفحة غير موجودة

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectUserData()
        {
            // Arrange
            var email = "test.user@gmail.com";
            var firstName = "Test";
            var lastName = "User";

            await CreatePendingUser(firstName, lastName, email);

            var query = new GetPendingUsersQuery(pageNumber: 1, pageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.Count);

            var user = result.Data.First();
            Assert.Equal(email, user.Email);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
        }

        [Fact]
        public async Task Handle_ShouldNotReturnApprovedUsers()
        {
            // Arrange
            await CreatePendingUsers(3);
            await CreateApprovedUsers(2);

            var query = new GetPendingUsersQuery(pageNumber: 1, pageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count); // المستخدمين المعلقين فقط
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

        private async Task CreatePendingUser(string firstName, string lastName, string email)
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
