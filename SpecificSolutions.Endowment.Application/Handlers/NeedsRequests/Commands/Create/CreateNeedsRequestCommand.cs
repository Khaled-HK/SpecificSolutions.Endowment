using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create
{
    public class CreateNeedsRequestCommand : CreateRequestCommand
    {
        public string NeedsType { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Provider { get; set; } = string.Empty;
    }
}