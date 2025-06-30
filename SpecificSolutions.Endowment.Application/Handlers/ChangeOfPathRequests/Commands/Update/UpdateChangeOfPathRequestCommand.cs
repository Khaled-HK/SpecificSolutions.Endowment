using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update
{
    public class UpdateChangeOfPathRequestCommand : UpdateRequestCommand
    {
        public string CurrentType { get; set; }
        public string NewType { get; set; }
        public string Reason { get; set; }
    }
}