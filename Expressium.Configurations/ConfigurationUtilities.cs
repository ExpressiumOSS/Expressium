using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expressium.Configurations
{
    public static class ConfigurationUtilities
    {
        public static T DeserializeAsJson<T>(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static void SerializeAsJson<T>(string filePath, T configuration)
        {
            var jsonString = JsonSerializer.Serialize(configuration, new JsonSerializerOptions() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            File.WriteAllText(filePath, jsonString);
        }
    }
}
