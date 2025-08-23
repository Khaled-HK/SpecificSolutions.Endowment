using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update
{
    [Authorize(Permissions = Permission.QuranicSchoolEdit)]
    public class UpdateQuranicSchoolCommand : ICommand
    {
        public Guid Id { get; set; }
        public string SchoolName { get; set; }
        public BuildingDTO Building { get; set; } // Include nested DTO
    }
}