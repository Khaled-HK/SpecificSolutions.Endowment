using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update
{
    public class UpdateChangeOfPathRequestCommand : UpdateRequestCommand
    {
        public string CurrentType { get; set; } = string.Empty;
        public string NewType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}