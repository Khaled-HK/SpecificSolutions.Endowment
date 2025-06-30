using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create
{
    public class CreateDemolitionRequestCommand : CreateRequestCommand
    {
        public string EstimatedCost { get; set; }
        public string EstimatedTime { get; set; }
        public string DemolitionReason { get; set; }
        public decimal EstimatedRebuildingCost { get; set; }
        public string ContractorName { get; set; }
        public string Reason { get; set; }
    }
}