using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Update
{
    public class UpdateNameChangeRequestCommand : UpdateRequestCommand
    {
        public string CurrentName { get; set; } = string.Empty;
        public string NewName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}