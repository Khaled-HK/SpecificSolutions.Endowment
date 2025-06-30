using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Update
{
    public class UpdateNameChangeRequestCommand : UpdateRequestCommand
    {
        public string CurrentName { get; set; }
        public string NewName { get; set; }
        public string Reason { get; set; }
    }
}