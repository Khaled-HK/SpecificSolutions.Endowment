using Dashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Infrastructure.Persistence;
using SpecificSolutions.Endowment.Test.Helper;

namespace SpecificSolutions.Endowment.Test
{
    public abstract class BaseTest : IClassFixture<WebApplicationFactory<Program>>//, IAsyncLifetime
    {
        protected HandlerHelper _handlerHelper;
        protected const string user_id = "a2d890d8-01d1-494b-9f62-6336b937e6fc";
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly Mock<ICurrentUser> _currentUserMock = new();
        private IServiceProvider _provider { get; }

        public BaseTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetUnitTestsDefaultEnvironment(currentUser: _currentUserMock.Object, serviceProvider: factory.Services);
                });
            });

            _provider = _factory.Services;

            _handlerHelper = new HandlerHelper(_factory.Services);

            //SeedDatabaseAsync();

            SetCurrentUser(user_id, "Khaled");
        }

        public void SetCurrentUser(string id, string userName)
        {
            _currentUserMock
                .Setup(x => x.Id)
                .Returns(id);

            _currentUserMock
                .Setup(x => x.UserName)
                .Returns(userName);

            _currentUserMock
                .Setup(x => x.HasPermissionAsync(It.IsAny<Permission>()))
                .ReturnsAsync(true); // Simulate permission check
        }

        public async Task<EndowmentResponse> Handle<T>(T command)
        {
            using var scope = _provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return (EndowmentResponse)await mediator.Send(command);
        }

        //protected void SeedDatabaseAsync()
        //{
        //    var scope = _provider.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    context.Database.Migrate();

        //    context.Accounts.RemoveRange(context.Accounts);
        //    context.Offices.RemoveRange(context.Offices);
        //    context.Requests.RemoveRange(context.Requests);
        //    context.ApplicationUser.RemoveRange(context.ApplicationUser);

        //    context.SaveChanges();
        //    context.ChangeTracker.Clear();

        //    context.Offices.Add(Office.Seed(
        //        id: new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
        //        userId: new Guid("a2d890d8-01d1-494b-9f62-6336b937e6fc"),
        //        name: "Main Office",
        //        location: "Tripoli",
        //        phoneNumber: "09284746974",
        //        regionId: new Guid("DDEC6E9E-7698-4623-9A84-4E5EFC02187C")));

        //    context.Users.Add(ApplicationUser.Seed(new Guid(user_id).ToString(),
        //                                        "1",
        //                                        "Khaled111",
        //                                        "Alnefati222",
        //                                        new Guid("DDEC6E9E-7628-4623-9A94-4E4EFC02187C"),
        //                                        "1",
        //                                        "1",
        //                                        true));

        //    context.SaveChanges();
        //}

        public async Task<TResult> Query<TResult>(Func<AppDbContext, Task<TResult>> query)
        {
            using var scope = _provider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await query(appDbContext);
        }
    }
}