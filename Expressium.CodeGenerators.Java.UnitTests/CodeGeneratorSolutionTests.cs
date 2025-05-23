using Expressium.Configurations;
using NUnit.Framework;
using System;
using System.IO;

namespace Expressium.CodeGenerators.Java.UnitTests
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
            configuration.SolutionPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "CodeGeneratorSolutionJava");
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.Java.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Cucumber.ToString();
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            if (Directory.Exists(configuration.SolutionPath))
                Directory.Delete(configuration.SolutionPath, true);

            Directory.CreateDirectory(configuration.SolutionPath);

            var codeGeneratorSolution = new CodeGeneratorSolution(configuration);
            codeGeneratorSolution.GenerateAll();
        }

        [Test]
        public void CodeGeneratorSolution_GenerateAll_Expressium_Files()
        {
            Assert.That(File.Exists(configuration.ConfigurationPath), Is.True, "CodeGeneratorSolution solution generation completed...");
            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "CodeGeneratorSolution solution generation completed...");
        }

        [Test]
        public void CodeGeneratorSolution_GenerateAll_Configuration_File()
        {
            var projectApiTestPath = $@"{configuration.SolutionPath}\src\test\java\Bases";
            var configFile = File.ReadAllText(Path.Combine(projectApiTestPath, "Configuration.java"));

            Assert.That(configFile, Does.Contain(configuration.Company), "CodeGeneratorSolution solution configuration contains Company...");
            Assert.That(configFile, Does.Contain(configuration.Project), "CodeGeneratorSolution solution configuration contains Project...");
            Assert.That(configFile, Does.Contain(configuration.ApplicationUrl), "CodeGeneratorSolution solution configuration contains URL...");
        }
    }
}
