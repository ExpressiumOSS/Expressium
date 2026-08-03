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
                { "$PROJECT$", configuration.Project.ToUpper() },
                { "$Namespace$", nameSpace },
                { "$Url$", configuration.ApplicationUrl },
                { "$BrowserType$", configuration.Enroller.BrowserType },
                { "$SolutionPath$", configuration.SolutionPath }
            };

            // Generate Solution Files...
            var solutionFileName = Path.GetFileName(directory).ToLower() + ".sln";
            WriteToFile(Path.Combine(directory, ".gitignore"), Resources.GitIgnore, mapOfProperties);
            WriteToFile(Path.Combine(directory, solutionFileName), Resources.SolutionFile, mapOfProperties);
            WriteToFile(Path.Combine(directory, $"BuildRunTests.bat"), Resources.BatchFileTests, mapOfProperties);

#if DEBUG
            // Include BDD Skills file for quick reference during development...
            WriteToFile(Path.Combine(directory, "skills", "BDD", "SKILL.md"), Resources.BddSkill, mapOfProperties);
#endif

            // Generate Solution Project Files...
            var projectPath = Path.Combine(directory, nameSpace);
            WriteToFile(Path.Combine(projectPath, nameSpace + ".csproj"), Resources.ProjectFileApi, mapOfProperties);

            var controlsPath = Path.Combine(projectPath, "Controls");
            WriteToFile(Path.Combine(controlsPath, "WebElements.cs"), Resources.WebElements, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebConditions.cs"), Resources.WebConditions, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebButton.cs"), Resources.WebButton, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebCheckBox.cs"), Resources.WebCheckBox, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebComboBox.cs"), Resources.WebComboBox, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebControl.cs"), Resources.WebControl, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebLink.cs"), Resources.WebLink, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebListBox.cs"), Resources.WebListBox, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebRadioButton.cs"), Resources.WebRadioButton, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebText.cs"), Resources.WebText, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebTextBox.cs"), Resources.WebTextBox, mapOfProperties);
            WriteToFile(Path.Combine(controlsPath, "WebTable.cs"), Resources.WebTable, mapOfProperties);

            Directory.CreateDirectory(Path.Combine(projectPath, CodeFolders.Models.ToString()));

            var pagesPath = Path.Combine(projectPath, "Pages");
            WriteToFile(Path.Combine(pagesPath, "BasePage.cs"), Resources.BasePage, mapOfProperties);
            WriteToFile(Path.Combine(pagesPath, "BaseTable.cs"), Resources.BaseTable, mapOfProperties);

#if DEBUG
            var servicesPath = Path.Combine(projectPath, "Services");
            WriteToFile(Path.Combine(servicesPath, "BaseService.cs"), Resources.BaseService, mapOfProperties);
#endif

            // Generate Solution Test Project Files...
            var testProjectPath = Path.Combine(directory, nameSpace + ".Tests");

            // Generate Test Project Root Files...
            WriteToFile(Path.Combine(testProjectPath, nameSpace + ".Tests.csproj"), Resources.ProjectFileApiTests, mapOfProperties);
            WriteToFile(Path.Combine(testProjectPath, "Configuration.json"), Resources.ConfigurationJson, mapOfProperties);
            WriteToFile(Path.Combine(testProjectPath, "AssemblyInfo.cs"), Resources.AssemblyInfo, mapOfProperties);
            WriteToFile(Path.Combine(testProjectPath, "Reqnroll.json"), Resources.ReqnrollJson, mapOfProperties);

            // Generate Test Project Factories Files...
            var factoriesPath = Path.Combine(testProjectPath, TestFolders.Factories.ToString());
            WriteToFile(Path.Combine(factoriesPath, "Randomizer.cs"), Resources.Randomizer, mapOfProperties);

            // Generate Test Project UiTests Files...
            var uitestsPath = Path.Combine(testProjectPath, TestFolders.UiTests.ToString());
            WriteToFile(Path.Combine(uitestsPath, "BasePageTest.cs"), Resources.BasePageTest, mapOfProperties);

#if DEBUG
            var apitestsPath = Path.Combine(testProjectPath, TestFolders.ApiTests.ToString());
            WriteToFile(Path.Combine(apitestsPath, "BaseServiceTest.cs"), Resources.BaseServiceTest, mapOfProperties);
#endif

            // Generate Test Project Infrastructure Files...
            var infrastructurePath = Path.Combine(testProjectPath, "Infrastructure");
            WriteToFile(Path.Combine(infrastructurePath, "BaseTestFixture.cs"), Resources.BaseTestFixture, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "BrowserFactory.cs"), Resources.BrowserFactory, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "BrowserManager.cs"), Resources.BrowserManager, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "Asserts.cs"), Resources.Asserts, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "Configuration.cs"), Resources.Configuration, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "ConfigurationLoader.cs"), Resources.ConfigurationLoader, mapOfProperties);
            WriteToFile(Path.Combine(infrastructurePath, "Logger.cs"), Resources.Logger, mapOfProperties);

            // Generate Test Project Feature Files...
            var featuresPath = Path.Combine(testProjectPath, TestFolders.BusinessTests.ToString(), "Features");
            WriteToFile(Path.Combine(featuresPath, "Login.feature"), Resources.Feature, mapOfProperties);

            // Generate Test Project Hook Files...
            var hooksPath = Path.Combine(testProjectPath, TestFolders.BusinessTests.ToString(), "Hooks");
            WriteToFile(Path.Combine(hooksPath, "BaseHooks.cs"), Resources.BaseHooks, mapOfProperties);
            WriteToFile(Path.Combine(hooksPath, "ReqnrollExtensions.cs"), Resources.ReqnrollExtensions, mapOfProperties);

            // Generate Test Project Step Files...
            var stepsPath = Path.Combine(testProjectPath, TestFolders.BusinessTests.ToString(), "Steps");
            WriteToFile(Path.Combine(stepsPath, "BaseContext.cs"), Resources.BaseContext, mapOfProperties);
            WriteToFile(Path.Combine(stepsPath, "BaseSteps.cs"), Resources.BaseSteps, mapOfProperties);
            WriteToFile(Path.Combine(stepsPath, "LoginSteps.cs"), Resources.LoginSteps, mapOfProperties);

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
