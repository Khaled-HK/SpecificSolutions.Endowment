using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Update
{
    public class UpdateNeedsRequestCommand : UpdateRequestCommand
    {
        public string NeedsType { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Provider { get; set; } = string.Empty;
    }
}