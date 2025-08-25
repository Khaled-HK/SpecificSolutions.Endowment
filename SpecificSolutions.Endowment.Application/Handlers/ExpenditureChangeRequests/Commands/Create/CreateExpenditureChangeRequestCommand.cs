using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Create
{
    public class CreateExpenditureChangeRequestCommand : CreateRequestCommand
    {
        public string CurrentExpenditure { get; set; } = string.Empty;
        public string NewExpenditure { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}