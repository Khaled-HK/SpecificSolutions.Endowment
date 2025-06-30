using System.Text.Json;

namespace SpecificSolutions.Endowment.Core.Helpers
{
    public static class Helper
    {
        //private static ISerializerService _serializerService;

        //public static void Initialize(ISerializerService serializerService)
        //{
        //    _serializerService = serializerService;
        //}

        //public static string Serialize<T>(T obj)
        //{
        //    return _serializerService.Serialize(obj);
        //}

        //public static T Deserialize<T>(string json)
        //{
        //    return _serializerService.Deserialize<T>(json);
        //}

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
