namespace SpecificSolutions.Endowment.Core.Common.Exceptions
{
    public abstract class EndowmentException : Exception
    {
        protected EndowmentException() : base() { }

        protected EndowmentException(string? message) : base(message) { }
    }
}
