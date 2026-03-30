using Expressium.Configurations;
using NUnit.Framework;
using System;
using System.IO;

namespace Expressium.CodeGenerators.Java.Playwright.UnitTests
{
    [TestFixture]
    public class CodeGeneratorSolutionTests
    {
        private Configuration configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Microsoft";
            configuration.Project = "Foodshop";
            configuration.ApplicationUrl = "http://www.dr.dk";
            configuration.SolutionPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "CodeGeneratorSolutionJavaPlaywright");
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.Java.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Playwright.ToString();

            if (Directory.Exists(configuration.SolutionPath))
                Directory.Delete(configuration.SolutionPath, true);

            Directory.CreateDirectory(configuration.SolutionPath);

            var codeGeneratorSolution = new CodeGeneratorSolution(configuration);
            codeGeneratorSolution.GenerateAll();
        }

        [Test]
        public void CodeGeneratorSolutionJavaPlaywright_GenerateAll_Expressium_Files()
        {
            Assert.That(File.Exists(configuration.ConfigurationPath), Is.True, "CodeGeneratorSolution solution generation completed...");
            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "CodeGeneratorSolution solution generation completed...");
        }

        [Test]
        public void CodeGeneratorSolutionJavaPlaywright_GenerateAll_Configuration_File()
        {
            var testResourcesPath = Path.Combine(configuration.SolutionPath, "src", "test", "resources");
            var configFile = File.ReadAllText(Path.Combine(testResourcesPath, "configuration.json"));

            Assert.That(configFile, Does.Contain(configuration.Company), "CodeGeneratorSolution solution configuration contains Company...");
            Assert.That(configFile, Does.Contain(configuration.Project), "CodeGeneratorSolution solution configuration contains Project...");
            Assert.That(configFile, Does.Contain(configuration.ApplicationUrl), "CodeGeneratorSolution solution configuration contains URL...");
        }

        [Test]
        public void CodeGeneratorSolutionJavaPlaywright_GenerateAll_PomXml()
        {
            var pomFile = Path.Combine(configuration.SolutionPath, "pom.xml");

            Assert.That(File.Exists(pomFile), Is.True, "CodeGeneratorSolution pom.xml generation completed...");
        }

        [Test]
        public void CodeGeneratorSolutionJavaPlaywright_GenerateAll_BasePage()
        {
            var packagePath = "microsoft/foodshop/web/api";
            var mainSourcePath = Path.Combine(configuration.SolutionPath, "src", "main", "java", packagePath);
            var basePageFile = Path.Combine(mainSourcePath, "BasePage.java");

            Assert.That(File.Exists(basePageFile), Is.True, "CodeGeneratorSolution BasePage.java generation completed...");
        }

        [Test]
        public void CodeGeneratorSolutionJavaPlaywright_GenerateAll_WebControls()
        {
            var packagePath = "microsoft/foodshop/web/api";
            var controlsPath = Path.Combine(configuration.SolutionPath, "src", "main", "java", packagePath, "controls");

            Assert.That(File.Exists(Path.Combine(controlsPath, "WebControl.java")), Is.True, "CodeGeneratorSolution WebControl.java generation completed...");
            Assert.That(File.Exists(Path.Combine(controlsPath, "WebButton.java")), Is.True, "CodeGeneratorSolution WebButton.java generation completed...");
            Assert.That(File.Exists(Path.Combine(controlsPath, "WebTextBox.java")), Is.True, "CodeGeneratorSolution WebTextBox.java generation completed...");
        }
    }
}
