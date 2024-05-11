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

            //if (!Directory.Exists(SolutionPath))
            //    throw new ArgumentException("The Configuration 'SolutionPath' directory does not exist...");

            //if (!File.Exists(ConfigurationPath))
            //    throw new ArgumentException("The Configuration 'ConfigurationPath' file does not exist...");

            //if (!File.Exists(RepositoryPath))
            //    throw new ArgumentException("The Configuration 'RepositoryPath' file does not exist...");

            Enroller.Validate();
            CodeGenerator.Validate();
        }

        public bool IsCodingLanguageCSharp()
        {
            if (CodeGenerator.CodingLanguage == CodingLanguages.CSharp.ToString())
                return true;

            return false;
        }

        public bool IsCodingLanguageJava()
        {
            if (CodeGenerator.CodingLanguage == CodingLanguages.Java.ToString())
                return true;

            return false;
        }

        public bool IsCodingStylePageFactory()
        {
            if (CodeGenerator.CodingStyle == CodingStyles.PageFactory.ToString())
                return true;

            return false;
        }

        public bool IsCodingStyleByLocators()
        {
            if (CodeGenerator.CodingStyle == CodingStyles.ByLocators.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourSpecflow()
        {
            if (CodeGenerator.CodingFlavour == CodingFlavours.Specflow.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourReqnroll()
        {
            if (CodeGenerator.CodingFlavour == CodingFlavours.Reqnroll.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourCucumber()
        {
            if (CodeGenerator.CodingFlavour == CodingFlavours.Cucumber.ToString())
                return true;

            return false;
        }

        public string GetNameSpace()
        {
            return $"{Company}.{Project}.Web.API";
        }

        public Configuration Copy()
        {
            var serialized = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Configuration>(serialized);
        }
    }
}
