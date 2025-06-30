using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update
{
    public class UpdateDemolitionRequestCommand : UpdateRequestCommand
    {
        public string EstimatedCost { get; set; }
        public string EstimatedTime { get; set; }
        public string DemolitionReason { get; set; }
        public decimal EstimatedRebuildingCost { get; set; }
        public string ContractorName { get; set; }
        public string Reason { get; set; }
    }
}