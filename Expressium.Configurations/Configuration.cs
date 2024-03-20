using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Expressium.Configurations
{
    public class Configuration
    {
        public string Company { get; set; }
        public string Project { get; set; }
        public string ApplicationUrl { get; set; }
        public string SolutionPath { get; set; }
        public string RepositoryPath { get; set; }
        public string CodingLanguage { get; set; }
        public string CodingFlavour { get; set; }
        public string CodingStyle { get; set; }

        public ConfigurationEnroller Enroller { get; set; }
        public ConfigurationCodeGenerator CodeGenerator { get; set; }

        public Configuration()
        {
            Enroller = new ConfigurationEnroller();
            CodeGenerator = new ConfigurationCodeGenerator();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Company))
                throw new ArgumentException(string.Format("The Configuration property 'Company' is undefined..."));

            if (string.IsNullOrWhiteSpace(Project))
                throw new ArgumentException(string.Format("The Configuration property 'Project' is undefined..."));

            if (string.IsNullOrWhiteSpace(ApplicationUrl))
                throw new ArgumentException(string.Format("The Configuration property 'ApplicationUrl' is undefined..."));

            if (string.IsNullOrWhiteSpace(SolutionPath))
                throw new ArgumentException(string.Format("The Configuration property 'SolutionPath' is undefined...'"));

            if (string.IsNullOrWhiteSpace(RepositoryPath))
                throw new ArgumentException(string.Format("The Configuration property 'RepositoryPath' is undefined...'"));

            if (!Directory.Exists(SolutionPath))
                throw new ArgumentException(string.Format("The Configuration property 'SolutionPath' does not exist..."));

            if (!File.Exists(RepositoryPath))
                throw new ArgumentException(string.Format("The Configuration property 'RepositoryPath' does not exist..."));

            if (!Enum.GetNames(typeof(CodingLanguages)).Any(e => CodingLanguage == e))
                throw new ArgumentException(string.Format("The Configuration property 'CodingLanguage' is invalid..."));

            foreach (var action in Enroller.Actions)
                action.Validate();
        }

        public bool IsCodingLanguageCSharp()
        {
            if (CodingLanguage == CodingLanguages.CSharp.ToString())
                return true;

            return false;
        }

        public bool IsCodingLanguageJava()
        {
            if (CodingLanguage == CodingLanguages.Java.ToString())
                return true;

            return false;
        }

        public bool IsCodingStylePageFactory()
        {
            if (CodingStyle == CodingStyles.PageFactory.ToString())
                return true;

            return false;
        }

        public bool IsCodingStyleByLocators()
        {
            if (CodingStyle == CodingStyles.ByLocators.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourSpecflow()
        {
            if (CodingFlavour == CodingFlavours.Specflow.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourReqnroll()
        {
            if (CodingFlavour == CodingFlavours.Reqnroll.ToString())
                return true;

            return false;
        }

        public bool IsCodingFlavourCucumber()
        {
            if (CodingFlavour == CodingFlavours.Cucumber.ToString())
                return true;

            return false;
        }

        public string GetNameSpace()
        {
            return string.Format("{0}.{1}.Web.API", Company, Project);
        }

        public Configuration Copy()
        {
            var serialized = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Configuration>(serialized);
        }
    }
}
