using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Create
{
    public class CreateConstructionRequestCommand : CreateRequestCommand
    {
        public string BuildingType { get; set; } = string.Empty;
        public string ProposedLocation { get; set; } = string.Empty;
        public double ProposedArea { get; set; }
        public double EstimatedCost { get; set; }
        public string ContractorName { get; set; } = string.Empty;
    }
}