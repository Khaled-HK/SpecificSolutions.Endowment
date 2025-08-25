using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update
{
    public class UpdateExpenditureChangeRequestCommand : UpdateRequestCommand
    {
        public string CurrentExpenditure { get; set; } = string.Empty;
        public string NewExpenditure { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}