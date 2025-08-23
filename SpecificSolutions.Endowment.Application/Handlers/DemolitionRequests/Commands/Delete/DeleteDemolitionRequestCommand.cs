using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.DemolitionRequestDelete)]
    public class DeleteDemolitionRequestCommand : ICommand
    {
        public Guid DemolitionRequestID { get; set; }
    }
}