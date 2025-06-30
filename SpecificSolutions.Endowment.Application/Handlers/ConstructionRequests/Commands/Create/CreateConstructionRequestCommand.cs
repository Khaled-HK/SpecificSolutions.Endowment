using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Create
{
    public class CreateConstructionRequestCommand : CreateRequestCommand
    {
        public string BuildingType { get; set; }
        public string ProposedLocation { get; set; }
        public double ProposedArea { get; set; }
        public double EstimatedCost { get; set; }
        public string ContractorName { get; set; }
    }
}