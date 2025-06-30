using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Core.Entities.Accounts;

namespace SpecificSolutions.Endowment.Test.Asserts.Accounts
{
    public static class AccountAssert
    {
        //Add Assert Equality to compare two Account objects
        public static void AssertEquality(Account accountFaker, Account dbAccount)
        {
            Assert.NotNull(dbAccount);
            Assert.Equal(accountFaker.Id, dbAccount.Id);
            Assert.Equal(accountFaker.Name, dbAccount.Name);
            Assert.Equal(accountFaker.MotherName, dbAccount.MotherName);
            Assert.Equal(accountFaker.BirthDate, dbAccount.BirthDate);
            Assert.Equal(accountFaker.AccountNumber, dbAccount.AccountNumber);
            Assert.Equal(accountFaker.Barcode, dbAccount.Barcode);
            Assert.Equal(accountFaker.Balance, dbAccount.Balance);
            Assert.Equal(accountFaker.BookNumber, dbAccount.BookNumber);
            Assert.Equal(accountFaker.City, dbAccount.City);
            Assert.Equal(accountFaker.ContactNumber, dbAccount.ContactNumber);
            Assert.Equal(accountFaker.Country, dbAccount.Country);
            Assert.Equal(accountFaker.Floors, dbAccount.Floors);
            Assert.Equal(accountFaker.AccountDetails, dbAccount.AccountDetails);
            Assert.Equal(accountFaker.Gender, dbAccount.Gender);
            Assert.Equal(accountFaker.IsActive, dbAccount.IsActive);
            Assert.Equal(accountFaker.LockerFileNumber, dbAccount.LockerFileNumber);
            Assert.Equal(accountFaker.LookOver, dbAccount.LookOver);
            Assert.Equal(accountFaker.NID, dbAccount.NID);
            Assert.Equal(accountFaker.Note, dbAccount.Note);
            Assert.Equal(accountFaker.PaperNumber, dbAccount.PaperNumber);
            Assert.Equal(accountFaker.RegistrationNumber, dbAccount.RegistrationNumber);
            Assert.Equal(accountFaker.SocialStatus, dbAccount.SocialStatus);
            Assert.Equal(accountFaker.Status, dbAccount.Status);
            Assert.Equal(accountFaker.Type, dbAccount.Type);
            Assert.Equal(accountFaker.UserId, dbAccount.UserId);
            Assert.Equal(accountFaker.Address, dbAccount.Address);
        }

        public static void AssertEquality(Account accountFaker, CreateAccountCommand command)
        {
            Assert.NotNull(command);
            Assert.Equal(accountFaker.Name, command.Name);
            Assert.Equal(accountFaker.MotherName, command.MotherName);
            Assert.Equal(accountFaker.BirthDate, command.BirthDate);
            Assert.Equal(accountFaker.AccountNumber, command.AccountNumber);
            Assert.Equal(accountFaker.Barcode, command.Barcode);
            Assert.Equal(accountFaker.Balance, command.Balance);
            Assert.Equal(accountFaker.BookNumber, command.BookNumber);
            Assert.Equal(accountFaker.City, command.City);
            Assert.Equal(accountFaker.ContactNumber, command.ContactNumber);
            Assert.Equal(accountFaker.Country, command.Country);
            Assert.Equal(accountFaker.Floors, command.Floors);
            Assert.Equal(accountFaker.Address, command.Address);
            Assert.Equal(accountFaker.Gender, command.Gender);
            Assert.Equal(accountFaker.IsActive, command.IsActive);
            Assert.Equal(accountFaker.LockerFileNumber, command.LockerFileNumber);
            Assert.Equal(accountFaker.LookOver, command.LookOver);
            Assert.Equal(accountFaker.NID, command.NID);
            Assert.Equal(accountFaker.Note, command.Note);
            Assert.Equal(accountFaker.PaperNumber, command.PaperNumber);
            Assert.Equal(accountFaker.RegistrationNumber, command.RegistrationNumber);
            Assert.Equal(accountFaker.SocialStatus, command.SocialStatus);
            Assert.Equal(accountFaker.Status, command.Status);
            Assert.Equal(accountFaker.Type, command.Type);
            Assert.Equal(accountFaker.UserId, command.UserId);
        }
    }
}
