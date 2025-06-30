using SpecificSolutions.Endowment.Core.Common.Exceptions;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Exceptions
{
    public class RequestAlreadyExistsException : EndowmentException
    {
        private const string _message = "Request with ReferenceNumber `{0}` already exists.";
        public RequestAlreadyExistsException(string referenceNumber)
            : base(string.Format(_message, referenceNumber))
        {
        }
    }
}
