using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update
{
    public class UpdateDemolitionRequestCommand : UpdateRequestCommand
    {
        public string EstimatedCost { get; set; } = string.Empty;
        public string EstimatedTime { get; set; } = string.Empty;
        public string DemolitionReason { get; set; } = string.Empty;
        public decimal EstimatedRebuildingCost { get; set; }
        public string ContractorName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}