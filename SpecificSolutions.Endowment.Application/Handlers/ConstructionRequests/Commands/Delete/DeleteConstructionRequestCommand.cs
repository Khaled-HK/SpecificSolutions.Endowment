using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.ConstructionRequestDelete)]
    public class DeleteConstructionRequestCommand : ICommand
    {
        public Guid ConstructionRequestID { get; set; }
    }
}