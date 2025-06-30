using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Create
{
    public class CreateExpenditureChangeRequestCommand : CreateRequestCommand
    {
        public string CurrentExpenditure { get; set; }
        public string NewExpenditure { get; set; }
        public string Reason { get; set; }
    }
}