using Dashboard;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Enums.Accounts;
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
            await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(createAccountCommandfaker, CancellationToken.None);

            // Assert
            var account = await Query(a => a.Accounts.FirstOrDefaultAsync());

            AccountAssert.AssertEquality(account, createAccountCommandfaker);
        }

        [Theory]
        [InlineData("Name", "", "Account name is required")]
        [InlineData("Name", null, "Account name is required")]
        [InlineData("Name", "   ", "Account name is required")]
        [InlineData("MotherName", "", "Mother's name is required")]
        [InlineData("MotherName", null, "Mother's name is required")]
        [InlineData("MotherName", "   ", "Mother's name is required")]
        [InlineData("Barcode", "", "Barcode is required")]
        [InlineData("Barcode", null, "Barcode is required")]
        [InlineData("Barcode", "   ", "Barcode is required")]
        [InlineData("AccountNumber", "", "Account number is required")]
        [InlineData("AccountNumber", null, "Account number is required")]
        [InlineData("AccountNumber", "   ", "Account number is required")]
        [InlineData("LockerFileNumber", 0, "Locker file number must be greater than zero")]
        [InlineData("LockerFileNumber", -1, "Locker file number must be greater than zero")]
        [InlineData("NID", 0, "National ID must be greater than zero")]
        [InlineData("NID", -1, "National ID must be greater than zero")]
        [InlineData("Balance", -1, "Balance cannot be negative")]
        [InlineData("Balance", -100.50, "Balance cannot be negative")]
        [InlineData("BookNumber", -1, "Book number cannot be negative")]
        [InlineData("BookNumber", -10, "Book number cannot be negative")]
        [InlineData("PaperNumber", -1, "Paper number cannot be negative")]
        [InlineData("PaperNumber", -5, "Paper number cannot be negative")]
        [InlineData("RegistrationNumber", -1, "Registration number cannot be negative")]
        [InlineData("RegistrationNumber", -20, "Registration number cannot be negative")]
        [InlineData("Floors", -1, "Number of floors cannot be negative")]
        [InlineData("Floors", -5, "Number of floors cannot be negative")]
        [InlineData("Floors", 101, "Number of floors cannot exceed 100")]
        [InlineData("ContactNumber", "123456789", "Contact number is invalid")]
        [InlineData("ContactNumber", "abc-def-ghi", "Contact number is invalid")]
        [InlineData("ContactNumber", "09-123456", "Contact number is invalid")]
        [InlineData("ContactNumber", "02-12345678", "Contact number is invalid")]
        [InlineData("BirthDate", "MinValue", "Birth date is required")]
        [InlineData("Gender", "Invalid", "Gender is invalid")]
        [InlineData("Status", "Invalid", "Status is invalid")]
        [InlineData("SocialStatus", "Invalid", "Social status is invalid")]
        [InlineData("AccountType", "Invalid", "Account type is invalid")]
        public async Task Handle_InvalidField_ReturnsValidationError(string fieldName, object invalidValue, string expectedMessage)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Set the invalid value for the specific field
            SetInvalidValue(command, fieldName, invalidValue);

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains(expectedMessage, result.Message);
        }

        [Theory]
        [InlineData("Name", "أحمد محمد علي", "Valid Arabic name")]
        [InlineData("Name", "John Smith", "Valid English name")]
        [InlineData("Name", "Ahmed123", "Valid name with numbers")]
        [InlineData("MotherName", "فاطمة أحمد", "Valid Arabic mother name")]
        [InlineData("MotherName", "Mary Johnson", "Valid English mother name")]
        [InlineData("Barcode", "123456789", "Valid barcode")]
        [InlineData("Barcode", "BAR-CODE-001", "Valid barcode with hyphens")]
        [InlineData("AccountNumber", "ACC-2024-001", "Valid account number")]
        [InlineData("AccountNumber", "123456789012", "Valid numeric account number")]
        [InlineData("LockerFileNumber", 1, "Minimum valid locker file number")]
        [InlineData("LockerFileNumber", 999999, "Large valid locker file number")]
        [InlineData("NID", 1, "Minimum valid NID")]
        [InlineData("NID", 999999999, "Large valid NID")]
        [InlineData("Balance", 0, "Zero balance")]
        [InlineData("Balance", 1000000.99, "Large valid balance")]
        [InlineData("BookNumber", 0, "Zero book number")]
        [InlineData("BookNumber", 999999, "Large valid book number")]
        [InlineData("PaperNumber", 0, "Zero paper number")]
        [InlineData("PaperNumber", 999999, "Large valid paper number")]
        [InlineData("RegistrationNumber", 0, "Zero registration number")]
        [InlineData("RegistrationNumber", 999999, "Large valid registration number")]
        [InlineData("Floors", 0, "Zero floors")]
        [InlineData("Floors", 100, "Maximum valid floors")]
        [InlineData("ContactNumber", "091-1234567", "Valid mobile number")]
        [InlineData("ContactNumber", "021-1234567", "Valid landline number")]
        [InlineData("BirthDate", "PastDate", "Valid past birth date")]
        [InlineData("Gender", "Male", "Valid male gender")]
        [InlineData("Gender", "Female", "Valid female gender")]
        [InlineData("Status", "Active", "Valid active status")]
        [InlineData("Status", "Inactive", "Valid inactive status")]
        [InlineData("SocialStatus", "Single", "Valid single status")]
        [InlineData("SocialStatus", "Married", "Valid married status")]
        [InlineData("AccountType", "Martyr", "Valid martyr account type")]
        [InlineData("AccountType", "Customer", "Valid customer account type")]
        public async Task Handle_ValidField_ReturnsSuccessResponse(string fieldName, object validValue, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Set the valid value for the specific field
            SetValidValue(command, fieldName, validValue);

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess, $"Test failed for {testDescription}: {result.Message}");
        }

        [Theory]
        [InlineData("Name", 201, "Account name must not exceed 200 characters")]
        [InlineData("MotherName", 201, "Mother's name must not exceed 200 characters")]
        [InlineData("Barcode", 51, "Barcode must not exceed 50 characters")]
        [InlineData("AccountNumber", 51, "Account number must not exceed 50 characters")]
        [InlineData("Note", 1001, "Note must not exceed 1000 characters")]
        [InlineData("Address", 501, "Address must not exceed 500 characters")]
        [InlineData("City", 101, "City must not exceed 100 characters")]
        [InlineData("Country", 101, "Country must not exceed 100 characters")]
        public async Task Handle_FieldExceedsMaxLength_ReturnsValidationError(string fieldName, int length, string expectedMessage)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Set the field to exceed maximum length
            SetFieldToMaxLength(command, fieldName, length);

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains(expectedMessage, result.Message);
        }

        [Theory]
        [InlineData("BirthDate", "FutureDate", "Birth date cannot be in the future")]
        [InlineData("BirthDate", "TooOldDate", "Birth date is invalid")]
        public async Task Handle_InvalidBirthDate_ReturnsValidationError(string fieldName, string dateType, string expectedMessage)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Set invalid birth date
            SetInvalidBirthDate(command, dateType);

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains(expectedMessage, result.Message);
        }

        [Fact]
        public async Task Handle_DuplicateAccountNumber_ReturnsValidationError()
        {
            // Arrange - Create first account
            var firstCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(firstCommand, CancellationToken.None);

            // Arrange - Create second account with same account number
            var secondCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.AccountNumber, firstCommand.AccountNumber)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(secondCommand, CancellationToken.None);

            // Note: This test assumes there's a unique constraint on AccountNumber
            // If not, this test would pass and we'd need to add the constraint
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_MultipleValidAccounts_ReturnsSuccessResponse()
        {
            // Arrange
            var commands = new List<CreateAccountCommand>();
            for (int i = 0; i < 5; i++)
            {
                var command = new CreateAccountCommandFaker()
                    .RuleFor(x => x.UserId, user_id)
                    .Generate();
                commands.Add(command);
            }

            // Act & Assert
            foreach (var command in commands)
            {
                var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);
                Assert.True(result.IsSuccess, $"Failed to create account {command.Name}: {result.Message}");
            }

            // Verify all accounts were created
            var accounts = await Query(a => a.Accounts.ToListAsync());
            Assert.Equal(5, accounts.Count);
        }

        [Fact]
        public async Task Handle_AccountWithAllOptionalFields_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.Note, "This is a detailed note about the account")
                .RuleFor(x => x.Address, "123 Main Street, Downtown Area")
                .RuleFor(x => x.City, "Tripoli")
                .RuleFor(x => x.Country, "Libya")
                .RuleFor(x => x.ContactNumber, "091-1234567")
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
            Assert.Equal(command.Note, account.Note);
            Assert.Equal(command.Address, account.Address);
            Assert.Equal(command.City, account.City);
            Assert.Equal(command.Country, account.Country);
            Assert.Equal(command.ContactNumber, account.ContactNumber);
        }

        [Fact]
        public async Task Handle_AccountWithMinimalData_ReturnsSuccessResponse()
        {
            // Arrange - Create account with only required fields
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.Note, "")
                .RuleFor(x => x.Address, "")
                .RuleFor(x => x.City, "")
                .RuleFor(x => x.Country, "")
                .RuleFor(x => x.ContactNumber, "")
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
            Assert.Equal("", account.Note);
            Assert.Equal("", account.Address);
            Assert.Equal("", account.City);
            Assert.Equal("", account.Country);
            Assert.Equal("", account.ContactNumber);
        }

        private void SetInvalidValue(CreateAccountCommand command, string fieldName, object invalidValue)
        {
            switch (fieldName)
            {
                case "Name":
                    command.Name = invalidValue?.ToString();
                    break;
                case "MotherName":
                    command.MotherName = invalidValue?.ToString();
                    break;
                case "Barcode":
                    command.Barcode = invalidValue?.ToString();
                    break;
                case "AccountNumber":
                    command.AccountNumber = invalidValue?.ToString();
                    break;
                case "LockerFileNumber":
                    command.LockerFileNumber = Convert.ToInt32(invalidValue);
                    break;
                case "NID":
                    command.NID = Convert.ToInt32(invalidValue);
                    break;
                case "Balance":
                    command.Balance = Convert.ToDecimal(invalidValue);
                    break;
                case "BookNumber":
                    command.BookNumber = Convert.ToInt32(invalidValue);
                    break;
                case "PaperNumber":
                    command.PaperNumber = Convert.ToInt32(invalidValue);
                    break;
                case "RegistrationNumber":
                    command.RegistrationNumber = Convert.ToInt32(invalidValue);
                    break;
                case "Floors":
                    command.Floors = Convert.ToInt32(invalidValue);
                    break;
                case "ContactNumber":
                    command.ContactNumber = invalidValue?.ToString();
                    break;
                case "BirthDate":
                    command.BirthDate = DateTime.MinValue;
                    break;
                case "Gender":
                    command.Gender = (Gender)999; // Invalid enum value
                    break;
                case "Status":
                    command.Status = (Status)999; // Invalid enum value
                    break;
                case "SocialStatus":
                    command.SocialStatus = (SocialStatus)999; // Invalid enum value
                    break;
                case "AccountType":
                    command.Type = (AccountType)999; // Invalid enum value
                    break;
                default:
                    throw new ArgumentException($"Unknown field name: {fieldName}");
            }
        }

        private void SetValidValue(CreateAccountCommand command, string fieldName, object validValue)
        {
            switch (fieldName)
            {
                case "Name":
                    command.Name = validValue.ToString();
                    break;
                case "MotherName":
                    command.MotherName = validValue.ToString();
                    break;
                case "Barcode":
                    command.Barcode = validValue.ToString();
                    break;
                case "AccountNumber":
                    command.AccountNumber = validValue.ToString();
                    break;
                case "LockerFileNumber":
                    command.LockerFileNumber = Convert.ToInt32(validValue);
                    break;
                case "NID":
                    command.NID = Convert.ToInt32(validValue);
                    break;
                case "Balance":
                    command.Balance = Convert.ToDecimal(validValue);
                    break;
                case "BookNumber":
                    command.BookNumber = Convert.ToInt32(validValue);
                    break;
                case "PaperNumber":
                    command.PaperNumber = Convert.ToInt32(validValue);
                    break;
                case "RegistrationNumber":
                    command.RegistrationNumber = Convert.ToInt32(validValue);
                    break;
                case "Floors":
                    command.Floors = Convert.ToInt32(validValue);
                    break;
                case "ContactNumber":
                    command.ContactNumber = validValue.ToString();
                    break;
                case "BirthDate":
                    command.BirthDate = DateTime.Today.AddYears(-30); // Valid past date
                    break;
                case "Gender":
                    command.Gender = validValue.ToString() == "Male" ? Gender.Male : Gender.Female;
                    break;
                case "Status":
                    command.Status = validValue.ToString() == "Active" ? Status.Active : Status.Inactive;
                    break;
                case "SocialStatus":
                    command.SocialStatus = validValue.ToString() == "Single" ? SocialStatus.Single : SocialStatus.Married;
                    break;
                case "AccountType":
                    command.Type = validValue.ToString() == "Martyr" ? AccountType.Martyr : AccountType.Customer;
                    break;
                default:
                    throw new ArgumentException($"Unknown field name: {fieldName}");
            }
        }

        private void SetFieldToMaxLength(CreateAccountCommand command, string fieldName, int length)
        {
            var longString = new string('A', length);
            switch (fieldName)
            {
                case "Name":
                    command.Name = longString;
                    break;
                case "MotherName":
                    command.MotherName = longString;
                    break;
                case "Barcode":
                    command.Barcode = longString;
                    break;
                case "AccountNumber":
                    command.AccountNumber = longString;
                    break;
                case "Note":
                    command.Note = longString;
                    break;
                case "Address":
                    command.Address = longString;
                    break;
                case "City":
                    command.City = longString;
                    break;
                case "Country":
                    command.Country = longString;
                    break;
                default:
                    throw new ArgumentException($"Unknown field name: {fieldName}");
            }
        }

        private void SetInvalidBirthDate(CreateAccountCommand command, string dateType)
        {
            switch (dateType)
            {
                case "FutureDate":
                    command.BirthDate = DateTime.Today.AddDays(1); // Future date
                    break;
                case "TooOldDate":
                    command.BirthDate = DateTime.Today.AddYears(-150); // Too old
                    break;
                default:
                    throw new ArgumentException($"Unknown date type: {dateType}");
            }
        }

        // ===== اختبارات إضافية للحالات المفقودة =====

        [Theory]
        [InlineData("Name", "Test123!@#", "Name contains invalid characters")]
        [InlineData("Name", "محمد@علي", "Name contains invalid characters")]
        [InlineData("MotherName", "Test123!@#", "Mother name contains invalid characters")]
        [InlineData("MotherName", "فاطمة@أحمد", "Mother name contains invalid characters")]
        public async Task Handle_InvalidNameCharacters_ReturnsValidationError(string fieldName, string invalidName, string expectedMessage)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            if (fieldName == "Name")
                command.Name = invalidName;
            else
                command.MotherName = invalidName;

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("invalid", result.Message.ToLower());
        }

        [Fact]
        public async Task Handle_DuplicateBarcode_ReturnsValidationError()
        {
            // Arrange - Create first account
            var firstCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(firstCommand, CancellationToken.None);

            // Arrange - Create second account with same barcode
            var secondCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.Barcode, firstCommand.Barcode)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(secondCommand, CancellationToken.None);

            // This assumes there should be a unique constraint on Barcode
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_DuplicateNID_ReturnsValidationError()
        {
            // Arrange - Create first account
            var firstCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(firstCommand, CancellationToken.None);

            // Arrange - Create second account with same NID
            var secondCommand = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.NID, firstCommand.NID)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(secondCommand, CancellationToken.None);

            // This assumes there should be a unique constraint on NID
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData("091-123456", "Mobile number too short")]
        [InlineData("091-12345678", "Mobile number too long")]
        [InlineData("021-123456", "Landline number too short")]
        [InlineData("021-12345678", "Landline number too long")]
        [InlineData("099-1234567", "Invalid area code for mobile")]
        [InlineData("029-1234567", "Invalid area code for landline")]
        public async Task Handle_InvalidPhoneNumberFormat_ReturnsValidationError(string phoneNumber, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.ContactNumber, phoneNumber)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess, $"Test failed for {testDescription}");
            Assert.Contains("invalid", result.Message.ToLower());
        }

        [Theory]
        [InlineData(true, "Account should be active")]
        [InlineData(false, "Account should be inactive")]
        public async Task Handle_DifferentIsActiveValues_ReturnsSuccessResponse(bool isActive, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.IsActive, isActive)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess, testDescription);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
            Assert.Equal(isActive, account.IsActive);
        }

        [Theory]
        [InlineData(true, "Account with lookover enabled")]
        [InlineData(false, "Account with lookover disabled")]
        public async Task Handle_DifferentLookOverValues_ReturnsSuccessResponse(bool lookOver, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.LookOver, lookOver)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess, testDescription);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
            Assert.Equal(lookOver, account.LookOver);
        }

        [Fact]
        public async Task Handle_AllAccountTypes_ReturnsSuccessResponse()
        {
            // Test all valid AccountType enum values
            var accountTypes = Enum.GetValues<AccountType>().Where(x => x != AccountType.All);

            foreach (var accountType in accountTypes)
            {
                // Arrange
                var command = new CreateAccountCommandFaker()
                    .RuleFor(x => x.UserId, user_id)
                    .RuleFor(x => x.Type, accountType)
                    .Generate();

                // Act
                var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

                // Assert
                Assert.True(result.IsSuccess, $"Failed for AccountType: {accountType}");

                var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
                Assert.NotNull(account);
                Assert.Equal(accountType, account.Type);
            }
        }

        [Fact]
        public async Task Handle_AllGenderValues_ReturnsSuccessResponse()
        {
            // Test all valid Gender enum values
            var genders = Enum.GetValues<Gender>();

            foreach (var gender in genders)
            {
                // Arrange
                var command = new CreateAccountCommandFaker()
                    .RuleFor(x => x.UserId, user_id)
                    .RuleFor(x => x.Gender, gender)
                    .Generate();

                // Act
                var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

                // Assert
                Assert.True(result.IsSuccess, $"Failed for Gender: {gender}");

                var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
                Assert.NotNull(account);
                Assert.Equal(gender, account.Gender);
            }
        }

        [Fact]
        public async Task Handle_AllSocialStatusValues_ReturnsSuccessResponse()
        {
            // Test all valid SocialStatus enum values
            var socialStatuses = Enum.GetValues<SocialStatus>();

            foreach (var socialStatus in socialStatuses)
            {
                // Arrange
                var command = new CreateAccountCommandFaker()
                    .RuleFor(x => x.UserId, user_id)
                    .RuleFor(x => x.SocialStatus, socialStatus)
                    .Generate();

                // Act
                var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

                // Assert
                Assert.True(result.IsSuccess, $"Failed for SocialStatus: {socialStatus}");

                var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
                Assert.NotNull(account);
                Assert.Equal(socialStatus, account.SocialStatus);
            }
        }

        [Fact]
        public async Task Handle_AllStatusValues_ReturnsSuccessResponse()
        {
            // Test all valid Status enum values
            var statuses = Enum.GetValues<Status>();

            foreach (var status in statuses)
            {
                // Arrange
                var command = new CreateAccountCommandFaker()
                    .RuleFor(x => x.UserId, user_id)
                    .RuleFor(x => x.Status, status)
                    .Generate();

                // Act
                var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

                // Assert
                Assert.True(result.IsSuccess, $"Failed for Status: {status}");

                var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
                Assert.NotNull(account);
                Assert.Equal(status, account.Status);
            }
        }

        [Theory]
        [InlineData(0.01, "Minimum positive balance")]
        [InlineData(999999.99, "Large balance")]
        [InlineData(100000.00, "Round number balance")]
        public async Task Handle_DifferentBalanceValues_ReturnsSuccessResponse(decimal balance, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.Balance, balance)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess, testDescription);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
            Assert.Equal(balance, account.Balance);
        }

        [Fact]
        public async Task Handle_AuditLogCreation_CreatesAuditLogEntry()
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var auditLog = await Query(a => a.AuditLogs.FirstOrDefaultAsync());
            Assert.NotNull(auditLog);
            Assert.Equal(user_id, auditLog.UserId);
            Assert.Contains("Account", auditLog.Context);
        }

        [Fact]
        public async Task Handle_NullUserId_ReturnsValidationError()
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, (string)null)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_EmptyUserId_ReturnsValidationError()
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, "")
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData("   ", "Name with only spaces")]
        [InlineData("\t\t", "Name with only tabs")]
        [InlineData("\n\n", "Name with only newlines")]
        public async Task Handle_WhitespaceOnlyName_ReturnsValidationError(string invalidName, string testDescription)
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.Name, invalidName)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess, testDescription);
            Assert.Contains("required", result.Message.ToLower());
        }

        [Fact]
        public async Task Handle_BirthDateExactlyToday_ReturnsValidationError()
        {
            // Arrange
            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .RuleFor(x => x.BirthDate, DateTime.Today)
                .Generate();

            // Act & Assert
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Contains("future", result.Message.ToLower());
        }

        // ===== اختبارات الصلاحيات =====

        [Fact]
        public async Task Handle_WithAccountAddPermission_ReturnsSuccessResponse()
        {
            // Arrange - المستخدم لديه صلاحية AccountAdd
            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountAdd))
                .ReturnsAsync(true);

            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act
            var result = await _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var account = await Query(a => a.Accounts.FirstOrDefaultAsync(x => x.Name == command.Name));
            Assert.NotNull(account);
        }

        [Fact]
        public async Task Handle_WithoutAccountAddPermission_ThrowsUnauthorizedException()
        {
            // Arrange - المستخدم ليس لديه صلاحية AccountAdd
            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountAdd))
                .ReturnsAsync(false);

            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None));

            Assert.Contains("You do not have permission to perform this action", exception.Message);
        }

        [Fact]
        public async Task Handle_WithOtherPermissionsButNotAccountAdd_ThrowsUnauthorizedException()
        {
            // Arrange - المستخدم لديه صلاحيات أخرى لكن ليس AccountAdd
            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountAdd))
                .ReturnsAsync(false);

            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountView))
                .ReturnsAsync(true);

            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountEdit))
                .ReturnsAsync(true);

            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None));

            Assert.Contains("You do not have permission to perform this action", exception.Message);
        }

        [Fact]
        public async Task Handle_WithNullUserIdAndPermission_ThrowsUnauthorizedException()
        {
            // Arrange - المستخدم ليس لديه ID وليس لديه صلاحية
            _currentUserMock
                .Setup(x => x.Id)
                .Returns((string)null);

            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountAdd))
                .ReturnsAsync(false);

            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None));

            Assert.Contains("You do not have permission to perform this action", exception.Message);
        }

        [Fact]
        public async Task Handle_WithEmptyUserIdAndPermission_ThrowsUnauthorizedException()
        {
            // Arrange - المستخدم لديه ID فارغ وليس لديه صلاحية
            _currentUserMock
                .Setup(x => x.Id)
                .Returns("");

            _currentUserMock
                .Setup(x => x.HasPermissionAsync(Permission.AccountAdd))
                .ReturnsAsync(false);

            var command = new CreateAccountCommandFaker()
                .RuleFor(x => x.UserId, user_id)
                .Generate();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handlerHelper.Handle<CreateAccountCommand, EndowmentResponse>(command, CancellationToken.None));

            Assert.Contains("You do not have permission to perform this action", exception.Message);
        }
    }
}
