using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.CodeGenerators.Java.Selenium.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java.Selenium
{
    internal class CodeGeneratorSolution : CodeGeneratorObject
    {
        internal CodeGeneratorSolution(Configuration configuration) : base(configuration, null)
        {
        }

        internal void GenerateAll()
        {
            var directory = configuration.SolutionPath;

            // Setup Solution Mapping Properties...
            var mapOfProperties = new Dictionary<string, string>
            {
                { "$Company$", configuration.Company },
                { "$Project$", configuration.Project },
                { "$Package$", GetPackage() },
                { "$PackagePath$", GetPackagePath() },
                { "$Url$", configuration.ApplicationUrl },
                { "$BrowserType$", configuration.Enroller.BrowserType }
            };

            // Generate Root Files...
            WriteToFile(Path.Combine(directory, ".gitignore"), Resources.GitIgnore, mapOfProperties);
            WriteToFile(Path.Combine(directory, "pom.xml"), Resources.PomXml, mapOfProperties);

            // Generate Main Source Files...
            var mainSourcePath = GetMainSourcePath();

            Directory.CreateDirectory(Path.Combine(mainSourcePath, CodeFolders.models.ToString()));
            Directory.CreateDirectory(Path.Combine(mainSourcePath, CodeFolders.pages.ToString()));

            WriteToFile(Path.Combine(mainSourcePath, "BasePage.java"), Resources.BasePage, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, "BaseTable.java"), Resources.BaseTable, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, "Logger.java"), Resources.Logger, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, "Randomizer.java"), Resources.Randomizer, mapOfProperties);

            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebControl.java"), Resources.WebControl, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebButton.java"), Resources.WebButton, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebCheckBox.java"), Resources.WebCheckBox, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebComboBox.java"), Resources.WebComboBox, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebLink.java"), Resources.WebLink, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebListBox.java"), Resources.WebListBox, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebRadioButton.java"), Resources.WebRadioButton, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebText.java"), Resources.WebText, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebTextBox.java"), Resources.WebTextBox, mapOfProperties);
            WriteToFile(Path.Combine(mainSourcePath, CodeFolders.controls.ToString(), "WebTable.java"), Resources.WebTable, mapOfProperties);

            // Generate Test Source Files...
            var testSourcePath = GetTestSourcePath();

            Directory.CreateDirectory(Path.Combine(testSourcePath, TestFolders.factories.ToString()));
            Directory.CreateDirectory(Path.Combine(testSourcePath, TestFolders.uitests.ToString()));

            WriteToFile(Path.Combine(testSourcePath, "BaseTest.java"), Resources.BaseTest, mapOfProperties);
            WriteToFile(Path.Combine(testSourcePath, "BaseTestFixture.java"), Resources.BaseTestFixture, mapOfProperties);
            WriteToFile(Path.Combine(testSourcePath, "BrowserFactory.java"), Resources.BrowserFactory, mapOfProperties);
            WriteToFile(Path.Combine(testSourcePath, "BrowserManager.java"), Resources.BrowserManager, mapOfProperties);
            WriteToFile(Path.Combine(testSourcePath, "Asserts.java"), Resources.Asserts, mapOfProperties);
            WriteToFile(Path.Combine(testSourcePath, "Configuration.java"), Resources.Configuration, mapOfProperties);

            // Generate Test Resources...
            var testResourcesPath = Path.Combine(directory, "src", "test", "resources");
            WriteToFile(Path.Combine(testResourcesPath, "configuration.json"), Resources.ConfigurationJson, mapOfProperties);

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
