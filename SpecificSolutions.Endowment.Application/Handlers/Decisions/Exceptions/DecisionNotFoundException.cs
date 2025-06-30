namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Exceptions
{
    public class DecisionNotFoundException : Exception
    {
        public DecisionNotFoundException(Guid id)
            : base($"Decision with ID {id} was not found.")
        {
        }
    }
}