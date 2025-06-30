using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Create
{
    public class CreateNameChangeRequestCommand : CreateRequestCommand
    {
        public string CurrentName { get; set; }
        public string NewName { get; set; }
        public string Reason { get; set; }
    }
}