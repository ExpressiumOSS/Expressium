using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.SolutionGenerators.Properties;
using System.Collections.Generic;
using System.IO;

namespace Expressium.SolutionGenerators.Java
{
    internal class SolutionGeneratorProjectJava : SolutionGeneratorProject
    {
        public SolutionGeneratorProjectJava(Configuration configuration) : base(configuration)
        {
        }

        internal override void GenerateAll()
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
            WriteToFile(Path.Combine(directory, "pom.xml"), Resources.PomJava, mapOfProperties);

            // Generate Solution Api Project Files...
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceApi, CodeFolders.Models.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceApi, CodeFolders.Pages.ToString()));

            var basesFolderApi = Path.Combine(directory, nameSpaceApi, "Bases");
            WriteToFile(Path.Combine(basesFolderApi, "Logger.java"), Resources.LoggerJava, mapOfProperties);

            if (configuration.IsCodingStyleByLocators())
            {
                WriteToFile(Path.Combine(basesFolderApi, "BasePage.java"), Resources.BasePageByLocatorsJava, mapOfProperties);
                WriteToFile(Path.Combine(basesFolderApi, "WebElements.java"), Resources.WebElementsByLocatorsJava, mapOfProperties);
            }
            else if (configuration.IsCodingStylePageFactory())
            {
                WriteToFile(Path.Combine(basesFolderApi, "BasePage.java"), Resources.BasePagePageFactoryJava, mapOfProperties);
                WriteToFile(Path.Combine(basesFolderApi, "WebElements.java"), Resources.WebElementsPageFactoryJava, mapOfProperties);
            }

            // Generate Solution Api Test Project Files...
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.Factories.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.UITests.ToString()));
            Directory.CreateDirectory(Path.Combine(directory, nameSpaceTest, TestFolders.BusinessTests.ToString()));

            var basesFolderTest = Path.Combine(directory, nameSpaceTest, "Bases");
            WriteToFile(Path.Combine(basesFolderTest, "BaseTestFixture.java"), Resources.BaseTestFixtureJava, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "BaseTest.java"), Resources.BaseTestJava, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "WebDrivers.java"), Resources.WebDriversJava, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "ScreenRecorderUtility.java"), Resources.ScreenRecorderUtilityJava, mapOfProperties);
            WriteToFile(Path.Combine(basesFolderTest, "Asserts.java"), Resources.AssertsJava, mapOfProperties);

            var BusinessTestsFolder = Path.Combine(directory, nameSpaceTest, TestFolders.BusinessTests.ToString());
            WriteToFile(Path.Combine(BusinessTestsFolder, "Features", "Login.feature"), Resources.FeatureJava, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "BaseSteps.java"), Resources.BaseStepsJava, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "BaseTestFixtureBDD.java"), Resources.BaseTestFixtureBDDJava, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "Steps", "LoginSteps.java"), Resources.LoginStepsJava, mapOfProperties);
            WriteToFile(Path.Combine(BusinessTestsFolder, "TestRunners", "BusinessTests.java"), Resources.TestRunnerJava, mapOfProperties);

            var resourcesFolder = Path.Combine(directory, @"src\test\resources");
            WriteToFile(Path.Combine(resourcesFolder, "BusinessTests.xml"), Resources.BddTestsJava, mapOfProperties);
            WriteToFile(Path.Combine(resourcesFolder, "RegressionTests.xml"), Resources.RegressionTestsJava, mapOfProperties);
            WriteToFile(Path.Combine(resourcesFolder, "UITests.xml"), Resources.TestRunnerUITestsJava, mapOfProperties);

            // Save Configuration & Object Repository Files...
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, new ObjectRepository());
            ConfigurationUtilities.SerializeAsJson(configuration.ConfigurationPath, configuration);
        }
    }
}
