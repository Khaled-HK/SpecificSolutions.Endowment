using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.ExpenditureChangeRequestDelete)]
    public class DeleteExpenditureChangeRequestCommand : ICommand
    {
        public Guid EndowmentExpenditureChangeRequestID { get; set; }
    }
}