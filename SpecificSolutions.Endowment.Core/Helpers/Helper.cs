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
            // Handle null objects
            if (obj == null)
                return "null";
            
            // Serialize the object to JSON
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string json)
        {
            // Handle null or empty JSON
            if (string.IsNullOrEmpty(json) || json == "null")
                return default(T);
            
            // Deserialize the JSON back to the object
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
