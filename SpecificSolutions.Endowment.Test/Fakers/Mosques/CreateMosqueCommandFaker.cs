using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Core.Enums.Buildings;
using SpecificSolutions.Endowment.Core.Enums.Mosques;

namespace SpecificSolutions.Endowment.Test.Fakers.Mosques
{
    public sealed class CreateMosqueCommandFaker : ParameterlessObjectFaker<CreateMosqueCommand>
    {
        public CreateMosqueCommandFaker()
        {
            RuleFor(x => x.Name, f => f.Company.CompanyName());
            RuleFor(x => x.RegionId, f => f.Random.Guid().ToString());
            RuleFor(x => x.OfficeId, f => f.Random.Guid().ToString());
            RuleFor(x => x.FileNumber, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.Definition, f => f.Lorem.Sentence());
            RuleFor(x => x.Classification, f => f.Lorem.Word());
            RuleFor(x => x.Unit, f => f.Lorem.Word());
            RuleFor(x => x.NearestLandmark, f => f.Address.StreetAddress());
            RuleFor(x => x.MapLocation, f => f.Address.Latitude() + "," + f.Address.Longitude());
            RuleFor(x => x.Sanitation, f => f.Lorem.Sentence());
            RuleFor(x => x.ElectricityMeter, f => f.Random.AlphaNumeric(10));
            RuleFor(x => x.AlternativeEnergySource, f => f.Lorem.Word());
            RuleFor(x => x.WaterSource, f => f.Lorem.Word());
            RuleFor(x => x.BriefDescription, f => f.Lorem.Paragraph());
            //RuleFor(x => x.PicturePath, f => f.Image.PlaceholderUrl());
            RuleFor(x => x.TotalCoveredArea, f => f.Random.Double(100, 1000));
            RuleFor(x => x.TotalLandArea, f => f.Random.Double(200, 2000));
            RuleFor(x => x.ServicesSpecialNeeds, f => f.Random.Bool());
            RuleFor(x => x.SpecialEntranceWomen, f => f.Random.Bool());
            RuleFor(x => x.NumberOfFloors, f => f.Random.Int(1, 5));
            RuleFor(x => x.OpeningDate, f => f.Date.Past(5));
            RuleFor(x => x.ConstructionDate, f => f.Date.Past(10));
            RuleFor(x => x.MosqueDefinition, f => f.PickRandom<MosqueDefinition>());
            RuleFor(x => x.MosqueClassification, f => f.PickRandom<MosqueClassification>());
            RuleFor(x => x.LandDonorName, f => f.Person.FullName);
            RuleFor(x => x.PrayerCapacity, f => f.Random.Int(100, 1000).ToString());
            RuleFor(x => x.SourceFunds, f => f.PickRandom<SourceFunds>());
            RuleFor(x => x.UserId, "a2d890d8-01d1-494b-9f62-6336b937e6fc");
        }
    }
}