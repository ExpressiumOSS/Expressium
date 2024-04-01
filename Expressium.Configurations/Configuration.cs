using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expressium.Configurations
{
    public class Configuration
    {
        public string Company { get; set; }
        public string Project { get; set; }
        public string ApplicationUrl { get; set; }
        public string SolutionPath { get; set; }

        [JsonIgnore]
        public string ConfigurationPath { get { return Path.Combine(SolutionPath, $"{Company}{Project}.cfg"); } }

        [JsonIgnore]
        public string RepositoryPath { get { return Path.Combine(SolutionPath, $"{Company}{Project}.repo"); } }

        public string CodingLanguage { get; set; }
        public string CodingFlavour { get; set; }
        public string CodingStyle { get; set; }

        public string BrowserType { get; set; }
        public string BrowserLanguage { get; set; }
        public bool BrowserMaximize { get; set; }
        public bool BrowserWindow { get; set; }
        public int BrowserWindowWidth { get; set; }
        public int BrowserWindowHeight { get; set; }

        public string NameLocator { get; set; }
        public string ControlsLocator { get; set; }

        public string CustomControls { get; set; }
        public string InitialLoginPage { get; set; }

        public bool IncludePages { get; set; }
        public bool IncludeTests { get; set; }
        public bool IncludeExtensions { get; set; }

        public List<ConfigurationAction> Actions { get; set; }

        public Configuration()
        {
            BrowserType = BrowserTypes.Chrome.ToString();
            BrowserMaximize = true;
            BrowserWindow = false;
            BrowserWindowWidth = 1920;
            BrowserWindowHeight = 1080;

            IncludePages = true;
            IncludeTests = true;
            IncludeExtensions = true;

            Actions = new List<ConfigurationAction>();
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

            if (string.IsNullOrWhiteSpace(ConfigurationPath))
                throw new ArgumentException("The Configuration property 'ConfigurationPath' is undefined...'");

            //if (!Directory.Exists(SolutionPath))
            //    throw new ArgumentException("The Configuration 'SolutionPath' directory does not exist...");

            if (!Enum.GetNames(typeof(CodingLanguages)).Any(e => CodingLanguage == e))
                throw new ArgumentException("The Configuration property 'CodingLanguage' is invalid...");

            if (!Enum.GetNames(typeof(CodingFlavours)).Any(e => CodingFlavour == e))
                throw new ArgumentException("The Configuration property 'CodingFlavour' is invalid...");

            if (!Enum.GetNames(typeof(CodingStyles)).Any(e => CodingStyle == e))
                throw new ArgumentException("The Configuration property 'CodingStyle' is invalid...");

            foreach (var action in Actions)
                action.Validate();
        }

        public bool IsActionAdded(ConfigurationAction action)
        {
            return Actions.Any(m => m.GetHashCode() == action.GetHashCode());
        }

        public ConfigurationAction GetAction(ConfigurationAction action)
        {
            if (IsActionAdded(action))
                return Actions.FirstOrDefault(c => c.GetHashCode() == action.GetHashCode());

            return null;
        }

        public void DeleteAction(ConfigurationAction action)
        {
            if (IsActionAdded(action))
            {
                int index = Actions.FindIndex(m => m.GetHashCode() == action.GetHashCode());
                Actions.RemoveAt(index);
            }
        }

        public void SwapActions(int masterId, int slaveId)
        {
            var action = Actions[masterId];
            Actions[masterId] = Actions[slaveId];
            Actions[slaveId] = action;
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
            return $"{Company}.{Project}.Web.API";
        }

        public Configuration Copy()
        {
            var serialized = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Configuration>(serialized);
        }
    }
}
