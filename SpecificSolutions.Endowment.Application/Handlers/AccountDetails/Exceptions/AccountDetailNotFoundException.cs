namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Exceptions
{
    public class AccountDetailNotFoundException : Exception
    {
        public AccountDetailNotFoundException(Guid id)
            : base($"AccountDetail with ID {id} was not found.")
        {
        }
    }
}

