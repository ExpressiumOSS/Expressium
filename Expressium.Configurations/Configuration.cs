using System;
using System.IO;
using System.Text.Json;

namespace Expressium.Configurations
{
    public class Configuration
    {
        public string Company { get; set; }
        public string Project { get; set; }
        public string ApplicationUrl { get; set; }
        public string SolutionPath { get; set; }
        public string ConfigurationPath { get { return Path.Combine(SolutionPath, $"{Company}{Project}.cfg"); } }
        public string RepositoryPath { get { return Path.Combine(SolutionPath, $"{Company}{Project}.repo"); } }

        public ConfigurationCodeGenerator CodeGenerator { get; set; }
        public ConfigurationEnroller Enroller { get; set; }

        public Configuration()
        {
            CodeGenerator = new ConfigurationCodeGenerator();
            Enroller = new ConfigurationEnroller();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Company))
                throw new ArgumentException("The Configuration property 'Company' is undefined...");

            if (string.IsNullOrWhiteSpace(Project))
                throw new ArgumentException("The Configuration property 'Project' is undefined...");

            if (string.IsNullOrWhiteSpace(ApplicationUrl))
                throw new ArgumentException("The Configuration property 'ApplicationUrl' is undefined...");

            if (string.IsNullOrWhiteSpace(SolutionPath))
                throw new ArgumentException("The Configuration property 'SolutionPath' is undefined...'");

            if (string.IsNullOrWhiteSpace(ConfigurationPath))
                throw new ArgumentException("The Configuration property 'ConfigurationPath' is undefined...'");

            if (string.IsNullOrWhiteSpace(RepositoryPath))
                throw new ArgumentException("The Configuration property 'RepositoryPath' is undefined...'");

            Enroller.Validate();
            CodeGenerator.Validate();
        }

        public Configuration Copy()
        {
            var serialized = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Configuration>(serialized);
        }
    }
}
