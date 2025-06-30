using System.Text.Json;

namespace SpecificSolutions.Endowment.Infrastructure.Services.BaseServices
{
    public static class SerializerService
    {
        public static string Serialize<T>(T obj)
        {
            // Serialize the object to JSON
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string json)
        {
            // Deserialize the JSON back to the object
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
