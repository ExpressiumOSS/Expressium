using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.CodeGenerators.CSharp.Selenium.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp.Selenium
{
    internal class CodeGeneratorSolution : CodeGeneratorObject
    {
        internal CodeGeneratorSolution(Configuration configuration) : base(configuration, null)
        {
        }

        internal void GenerateAll()
        {
            var directory = configuration.SolutionPath;
            var nameSpace = GetNameSpace();

            // Setup Solution Mapping Properties...
            var mapOfProperties = new Dictionary<string, string>
            {
                { "$Company$", configuration.Company },
                { "$Project$", configuration.Project },
                { "$Url$", configuration.ApplicationUrl },
                { "$BrowserType$", configuration.Enroller.BrowserType }
            };

            // Generate Solution Files...
            var solutionFileName = Path.GetFileName(directory).ToLower() + ".sln";
            WriteToFile(Path.Combine(directory, solutionFileName), Resources.SolutionFile, mapOfProperties);
            WriteToFile(Path.Combine(directory, "RegressionTest.bat"), Resources.RegressionTest, mapOfProperties);

            // Generate Solution Api Project Files...
            var apiProjectPath = Path.Combine(directory, nameSpace);

            Directory.CreateDirectory(Path.Combine(apiProjectPath, CodeFolders.Models.ToString()));
            Directory.CreateDirectory(Path.Combine(apiProjectPath, CodeFolders.Pages.ToString()));

            WriteToFile(Path.Combine(apiProjectPath, nameSpace + ".csproj"), Resources.ProjectFileApi, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "Logger.cs"), Resources.Logger, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "Randomizer.cs"), Resources.Randomizer, mapOfProperties);

            WriteToFile(Path.Combine(apiProjectPath, "WebElements.cs"), Resources.WebElements, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "WebConditions.cs"), Resources.WebConditions, mapOfProperties);

            WriteToFile(Path.Combine(apiProjectPath, "BasePage.cs"), Resources.BasePage, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "BaseTable.cs"), Resources.BaseTable, mapOfProperties);

            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebButton.cs"), Resources.WebButton, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebCheckBox.cs"), Resources.WebCheckBox, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebComboBox.cs"), Resources.WebComboBox, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebControl.cs"), Resources.WebControl, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebLink.cs"), Resources.WebLink, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebListBox.cs"), Resources.WebListBox, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebRadioButton.cs"), Resources.WebRadioButton, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebText.cs"), Resources.WebText, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebTextBox.cs"), Resources.WebTextBox, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath + "\\Controls", "WebTable.cs"), Resources.WebTable, mapOfProperties);

            // Generate Solution Api Test Project Files...
            var apiTestProjectPath = Path.Combine(directory, nameSpace + ".Tests");

            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.Factories.ToString()));
            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.UITests.ToString()));
            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString()));

            WriteToFile(Path.Combine(apiTestProjectPath, "BaseTestFixture.cs"), Resources.BaseTestFixture, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "BaseTest.cs"), Resources.BaseTest, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "BrowserFactory.cs"), Resources.BrowserFactory, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "BrowserManager.cs"), Resources.BrowserManager, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "Asserts.cs"), Resources.Asserts, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "Configuration.cs"), Resources.Configuration, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "configuration.json"), Resources.ConfigurationJson, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "AssemblyInfo.cs"), Resources.AssemblyInfo, mapOfProperties);

            WriteToFile(Path.Combine(apiTestProjectPath, nameSpace + ".Tests.csproj"), Resources.ProjectFileApiTests, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "ReqnRoll.json"), Resources.ReqnRollJson, mapOfProperties);

            var featureHookFiles = Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString(), "Hooks");
            var featureFiles = Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString(), "Features");
            var featureStepFiles = Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString(), "Steps");

            WriteToFile(Path.Combine(featureHookFiles, "BaseHooks.cs"), Resources.BaseHooks, mapOfProperties);
            WriteToFile(Path.Combine(featureHookFiles, "ReqnRollExtensions.cs"), Resources.ReqnRollExtensions, mapOfProperties);
            WriteToFile(Path.Combine(featureFiles, "Login.feature"), Resources.Feature, mapOfProperties);
            WriteToFile(Path.Combine(featureStepFiles, "BaseContext.cs"), Resources.BaseContext, mapOfProperties);
            WriteToFile(Path.Combine(featureStepFiles, "BaseSteps.cs"), Resources.BaseSteps, mapOfProperties);
            WriteToFile(Path.Combine(featureStepFiles, "LoginSteps.cs"), Resources.LoginSteps, mapOfProperties);

            // Save Configuration & Object Repository Files...
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, new ObjectRepository());
            ConfigurationUtilities.SerializeAsJson(configuration.ConfigurationPath, configuration);
        }

        protected static void WriteToFile(string destinationFile, string text, Dictionary<string, string> mapOfProperties)
        {
            foreach (var property in mapOfProperties)
                text = text.Replace(property.Key, property.Value);

            var directory = Path.GetDirectoryName(destinationFile);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fileStream = File.Create(destinationFile))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(text);
            }

            Console.WriteLine(destinationFile);
        }
    }
}
