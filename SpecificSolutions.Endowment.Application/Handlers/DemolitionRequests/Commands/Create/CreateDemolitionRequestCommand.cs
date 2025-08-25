using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create
{
    public class CreateDemolitionRequestCommand : CreateRequestCommand
    {
        public string EstimatedCost { get; set; } = string.Empty;
        public string EstimatedTime { get; set; } = string.Empty;
        public string DemolitionReason { get; set; } = string.Empty;
        public decimal EstimatedRebuildingCost { get; set; }
        public string ContractorName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}