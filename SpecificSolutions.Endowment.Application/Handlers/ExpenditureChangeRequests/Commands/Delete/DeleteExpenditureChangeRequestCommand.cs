using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Delete
{
    public class DeleteExpenditureChangeRequestCommand : ICommand
    {
        public Guid EndowmentExpenditureChangeRequestID { get; set; }
    }
}