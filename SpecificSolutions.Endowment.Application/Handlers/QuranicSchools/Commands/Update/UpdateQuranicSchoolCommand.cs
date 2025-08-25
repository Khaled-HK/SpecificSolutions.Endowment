using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.QuranicSchools;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update
{
    [Authorize(Permissions = Permission.QuranicSchoolUpdate)]
    public class UpdateQuranicSchoolCommand : ICommand, IUpdateQuranicSchoolCommand
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Location { get; set; }
        public required string ContactInfo { get; set; }
        public int Capacity { get; set; }
        public required string Status { get; set; }
        public required string PicturePath { get; set; }
        public required string LandDonorName { get; set; }
        public required string PrayerCapacity { get; set; }
    }
}