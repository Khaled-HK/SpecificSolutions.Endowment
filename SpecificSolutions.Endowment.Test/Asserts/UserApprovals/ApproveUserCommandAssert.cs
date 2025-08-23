using FluentValidation.TestHelper;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser;

namespace SpecificSolutions.Endowment.Test.Asserts.UserApprovals
{
    public static class ApproveUserCommandAssert
    {
        public static void AssertValidCommand(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        public static void AssertInvalidUserId(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
            result.ShouldNotHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertInvalidRoleName(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertMultipleValidationErrors(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertValidAdminRole(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        public static void AssertValidEmployeeRole(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        public static void AssertValidCustomerRole(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        public static void AssertCaseInsensitiveRole(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        public static void AssertInvalidRole(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertEmptyOrNullValidation(TestValidationResult<ApproveUserCommand> result, string fieldName)
        {
            Assert.False(result.IsValid);
            
            if (fieldName.ToLower() == "userid")
            {
                result.ShouldHaveValidationErrorFor(x => x.UserId);
                result.ShouldNotHaveValidationErrorFor(x => x.RoleName);
            }
            else if (fieldName.ToLower() == "rolename")
            {
                result.ShouldNotHaveValidationErrorFor(x => x.UserId);
                result.ShouldHaveValidationErrorFor(x => x.RoleName);
            }
        }

        public static void AssertWhitespaceValidation(TestValidationResult<ApproveUserCommand> result, string fieldName)
        {
            Assert.False(result.IsValid);
            
            if (fieldName.ToLower() == "userid")
            {
                result.ShouldHaveValidationErrorFor(x => x.UserId);
                result.ShouldNotHaveValidationErrorFor(x => x.RoleName);
            }
            else if (fieldName.ToLower() == "rolename")
            {
                result.ShouldNotHaveValidationErrorFor(x => x.UserId);
                result.ShouldHaveValidationErrorFor(x => x.RoleName);
            }
        }

        public static void AssertSpecialCharactersValidation(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertLongStringValidation(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertInvalidRoleFormat(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.False(result.IsValid);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        public static void AssertValidRoleFormat(TestValidationResult<ApproveUserCommand> result)
        {
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
