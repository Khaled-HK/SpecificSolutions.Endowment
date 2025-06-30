using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update
{
    public class UpdateExpenditureChangeRequestCommand : UpdateRequestCommand
    {
        public string CurrentExpenditure { get; set; }
        public string NewExpenditure { get; set; }
        public string Reason { get; set; }
    }
}