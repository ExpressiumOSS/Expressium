using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expressium.ObjectRepositories
{
    public static class ObjectRepositoryUtilities
    {
        public static T DeserializeAsJson<T>(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static void SerializeAsJson<T>(string filePath, T objectRepository)
        {
            var jsonString = JsonSerializer.Serialize(objectRepository, new JsonSerializerOptions() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            File.WriteAllText(filePath, jsonString);
        }
    }
}
