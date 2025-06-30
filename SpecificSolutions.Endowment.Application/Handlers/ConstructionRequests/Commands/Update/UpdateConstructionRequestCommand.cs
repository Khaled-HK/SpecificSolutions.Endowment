using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Update
{
    public class UpdateConstructionRequestCommand : UpdateRequestCommand
    {
        public string BuildingType { get; set; }
        public string ProposedLocation { get; set; }
        public double ProposedArea { get; set; }
        public double EstimatedCost { get; set; }
        public string ContractorName { get; set; }
    }
}