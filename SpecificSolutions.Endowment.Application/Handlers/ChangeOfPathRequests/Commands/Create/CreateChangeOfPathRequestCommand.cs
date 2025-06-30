using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Create
{
    public class CreateChangeOfPathRequestCommand : CreateRequestCommand
    {
        public string CurrentType { get; set; }
        public string NewType { get; set; }
        public string Reason { get; set; }
    }
}