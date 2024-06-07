using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.SolutionGenerators.Properties;
using System.Collections.Generic;
using System.IO;

namespace Expressium.SolutionGenerators.CSharp
{
    internal class SolutionGeneratorProjectCSharp : SolutionGeneratorProject
    {
        public SolutionGeneratorProjectCSharp(Configuration configuration) : base(configuration)
        {
        }

        internal override void GenerateAll()
        {
            var directory = configuration.SolutionPath;
            var nameSpace = configuration.GetNameSpace();

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
            WriteToFile(Path.Combine(directory, solutionFileName), Resources.SolutionFileCSharp, mapOfProperties);
            WriteToFile(Path.Combine(directory, "RegressionTest.bat"), Resources.RegressionTestCSharp, mapOfProperties);
            WriteToFile(Path.Combine(directory, "LivingDoc.bat"), Resources.LivingDocCSharp, mapOfProperties);

            // Generate Solution Api Project Files...
            var apiProjectPath = Path.Combine(directory, nameSpace);

            Directory.CreateDirectory(Path.Combine(apiProjectPath, CodeFolders.Models.ToString()));
            Directory.CreateDirectory(Path.Combine(apiProjectPath, CodeFolders.Pages.ToString()));

            WriteToFile(Path.Combine(apiProjectPath, "Logger.cs"), Resources.LoggerCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "Randomizer.cs"), Resources.RandomizerCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "WebElements.cs"), Resources.WebElementsCSharp, mapOfProperties);

            WriteToFile(Path.Combine(apiProjectPath, nameSpace + ".csproj"), Resources.ProjectFileApiCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "BasePage.cs"), Resources.BasePageCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiProjectPath, "BaseTable.cs"), Resources.BaseTableCSharp, mapOfProperties);

            // Generate Solution Api Test Project Files...
            var apiTestProjectPath = Path.Combine(directory, nameSpace + ".Tests");

            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.Factories.ToString()));
            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.UITests.ToString()));
            Directory.CreateDirectory(Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString()));

            WriteToFile(Path.Combine(apiTestProjectPath, "BaseTestFixture.cs"), Resources.BaseTestFixtureCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "BaseTest.cs"), Resources.BaseTestCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "WebDriverFactory.cs"), Resources.WebDriverFactoryCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "Asserts.cs"), Resources.AssertsCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "Configuration.cs"), Resources.ConfigurationCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "configuration.json"), Resources.ConfigurationTestsCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "ContextController.cs"), Resources.ContextControllerCSharp, mapOfProperties);
            WriteToFile(Path.Combine(apiTestProjectPath, "AssemblyInfo.cs"), Resources.AssemblyInfoCSharp, mapOfProperties);

            if (configuration.IsCodingFlavourSpecflow())
            {
                WriteToFile(Path.Combine(apiTestProjectPath, nameSpace + ".Tests.csproj"), Resources.ProjectFileApiTestsCSharp, mapOfProperties);
                WriteToFile(Path.Combine(apiTestProjectPath, "BaseHooks.cs"), Resources.BaseHooksCSharp, mapOfProperties);
            }
            else if (configuration.IsCodingFlavourReqnroll())
            {
                WriteToFile(Path.Combine(apiTestProjectPath, nameSpace + ".Tests.csproj"), Resources.ProjectFileApiTestsReqnRollCSharp, mapOfProperties);
                WriteToFile(Path.Combine(apiTestProjectPath, "BaseHooks.cs"), Resources.BaseHooksReqnRollCSharp, mapOfProperties);
            }
            else
            {
            }

            var featureFiles = Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString(), "Features");
            var featureStepFiles = Path.Combine(apiTestProjectPath, TestFolders.BusinessTests.ToString(), "Steps");

            WriteToFile(Path.Combine(featureFiles, "Login.feature"), Resources.FeatureCSharp, mapOfProperties);
            WriteToFile(Path.Combine(featureStepFiles, "BaseSteps.cs"), Resources.BaseStepsCSharp, mapOfProperties);

            if (configuration.IsCodingFlavourSpecflow())
                WriteToFile(Path.Combine(featureStepFiles, "LoginSteps.cs"), Resources.LoginStepsCSharp, mapOfProperties);
            else if (configuration.IsCodingFlavourReqnroll())
                WriteToFile(Path.Combine(featureStepFiles, "LoginSteps.cs"), Resources.LoginStepsReqnRollCSharp, mapOfProperties);
            else
            {
            }

            // Save Configuration & Object Repository Files...
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, new ObjectRepository());
            ConfigurationUtilities.SerializeAsJson(configuration.ConfigurationPath, configuration);
        }
    }
}
