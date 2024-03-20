using System.Text.Json.Serialization;

namespace Expressium.Configurations
{
    public class ConfigurationCodeGenerator
    {
        public bool Overwrites { get; set; }
        public bool Pages { get; set; }
        public bool PageTests { get; set; }
        public bool Extensions { get; set; }

        [JsonIgnore]
        public string InitialLoginPage { get; set; }

        public ConfigurationCodeGenerator()
        {
            Overwrites = true;
            Pages = true;
            PageTests = true;
            Extensions = true;
        }
    }
}
