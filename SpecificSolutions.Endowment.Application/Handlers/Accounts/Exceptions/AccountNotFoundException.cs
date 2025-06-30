namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(Guid id)
            : base($"Account with ID {id} was not found.")
        {
        }
    }
}