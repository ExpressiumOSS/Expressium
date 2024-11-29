using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.CodeGenerators.Java.Properties;
using System.Collections.Generic;
using System.IO;
using System;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorSolution : CodeGeneratorObject
    {
        internal CodeGeneratorSolution(Configuration configuration) : base(configuration, null)
        {
        }

        internal void GenerateAll()
        {
            var directory = configuration.SolutionPath;
            var nameSpaceApi = @"src\main\java";
            var nameSpaceTest = @"src\test\java";

            // Setup Solution Mapping Properties...
            var mapOfProperties = new Dictionary<string, string>
            {
                { "$Company$", configuration.Company },
                { "$Project$", configuration.Project },
                { "$Url$", configuration.ApplicationUrl },
                { "$BrowserType$", configuration.Enroller.BrowserType }
            };

            // Generate Solution Files...
            WriteToFile(Path.Combine(directory, "pom.xml"), Resources.Pom, mapOfProperties);

            // Generate Solution Api Project Files...
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceApi, CodeFolders.Models.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceApi, CodeFolders.Pages.ToString()));

            var basesFolderApi = Path.Combine(directory, nameSpaceApi, "Bases");
            WriteToFile(Path.Combine(basesFolderApi, "Logger.java"), Resources.Logger, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderApi, "WebElements.java"), Resources.WebElements, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderApi, "Randomizer.java"), Resources.Randomizer, mapOfProperties);

            if (IsCodingStyleByLocators())
            {
                WriteToFile(Path.Combine(basesFolderApi, "BasePage.java"), Resources.BasePageByLocators, mapOfProperties);
                WriteToFile(Path.Combine(basesFolderApi, "BaseTable.java"), Resources.BaseTable, mapOfProperties);
            }
            else if (IsCodingStylePageFactory())
            {
                WriteToFile(Path.Combine(basesFolderApi, "BasePage.java"), Resources.BasePagePageFactory, mapOfProperties);
                WriteToFile(Path.Combine(basesFolderApi, "BaseTable.java"), Resources.BaseTable, mapOfProperties);
            }
            else if (IsCodingStyleByControls())
            {
                WriteToFile(Path.Combine(basesFolderApi, "BasePage.java"), Resources.BasePageByControls, mapOfProperties);
                WriteToFile(Path.Combine(basesFolderApi, "BaseTable.java"), Resources.BaseTableByControls, mapOfProperties);

                var controlsFolderApi = Path.Combine(directory, nameSpaceApi, "Controls");
                WriteToFile(Path.Combine(controlsFolderApi, "WebButton.java"), Resources.WebButton, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebCheckBox.java"), Resources.WebCheckBox, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebComboBox.java"), Resources.WebComboBox, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebControl.java"), Resources.WebControl, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebLink.java"), Resources.WebLink, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebListBox.java"), Resources.WebListBox, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebRadioButton.java"), Resources.WebRadioButton, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebText.java"), Resources.WebText, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebTextBox.java"), Resources.WebTextBox, mapOfProperties);
                WriteToFile(Path.Combine(controlsFolderApi, "WebTable.java"), Resources.WebTable, mapOfProperties);
            }

            // Generate Solution Api Test Project Files...
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.Factories.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.UITests.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.BusinessTests.ToString()));

            var basesFolderTest = Path.Combine(directory, nameSpaceTest, "Bases");
            WriteToFile(Path.Combine(basesFolderTest, "BaseTestFixture.java"), Resources.BaseTestFixture, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "BaseTest.java"), Resources.BaseTest, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "WebDrivers.java"), Resources.WebDrivers, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "ScreenRecorderUtility.java"), Resources.ScreenRecorderUtility, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "Asserts.java"), Resources.Asserts, mapOfProperties);

            var BusinessTestsFolder = Path.Combine(directory, nameSpaceTest, TestFolders.BusinessTests.ToString());
            WriteToFile(Path.Combine(BusinessTestsFolder, "Features", "Login.feature"), Resources.Feature, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "BaseSteps.java"), Resources.BaseSteps, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "BaseTestFixtureBDD.java"), Resources.BaseTestFixtureBDD, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "LoginSteps.java"), Resources.LoginSteps, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "TestRunners", "BusinessTests.java"), Resources.TestRunner, mapOfProperties);

            var resourcesFolder = Path.Combine(directory, @"src\test\resources");
            WriteToFile(Path.Combine(resourcesFolder, "BusinessTests.xml"), Resources.BddTests, mapOfProperties);
            WriteToFile(Path.Combine(resourcesFolder, "RegressionTests.xml"), Resources.RegressionTests, mapOfProperties);
            WriteToFile(Path.Combine(resourcesFolder, "UITests.xml"), Resources.TestRunnerUITests, mapOfProperties);

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

