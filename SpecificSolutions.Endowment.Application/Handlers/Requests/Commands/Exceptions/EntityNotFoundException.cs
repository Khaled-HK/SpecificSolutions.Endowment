using SpecificSolutions.Endowment.Core.Common.Exceptions;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Exceptions
{
    public class EntityNotFoundException : EndowmentException
    {
        private const string _message = "The entity with ID `{0}` was not found.";

        public EntityNotFoundException(Guid entityId)
            : base(string.Format(_message, entityId))
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}