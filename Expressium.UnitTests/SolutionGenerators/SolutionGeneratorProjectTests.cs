using NUnit.Framework;
using Expressium.Configurations;
using Expressium.SolutionGenerators;
using System;
using System.IO;

namespace Expressium.UnitTests.SolutionGenerators
{
    [TestFixture]
    public class SolutionGeneratorProjectTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void SolutionGenerator_Invalid_Configuration()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            var exception = Assert.Throws<ArgumentException>(() => new SolutionGenerator(configuration));
            Assert.That(exception.Message, Is.EqualTo("The Configuration property 'ApplicationUrl' is undefined..."), "SolutionGenerator configuration is invalid...");
        }

        [Test]
        public void SolutionGenerator_GenerateAll_CSharp()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            configuration.ApplicationUrl = "http://www.dr.dk";
            configuration.SolutionPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "SolutionGenerator");
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.CSharp.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Specflow.ToString();
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            Directory.CreateDirectory(configuration.SolutionPath);

            var solutionGenerator = new SolutionGenerator(configuration);
            solutionGenerator.GenerateAll();

            Assert.That(File.Exists(configuration.ConfigurationPath), Is.True, "SolutionGenerator solution generation completed...");
            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "SolutionGenerator solution generation completed...");
        }

        [Test]
        public void SolutionGenerator_GenerateAll_Java()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            configuration.ApplicationUrl = "http://www.dr.dk";
            configuration.SolutionPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "SolutionGenerator");
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.Java.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Cucumber.ToString();
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            Directory.CreateDirectory(configuration.SolutionPath);

            var solutionGenerator = new SolutionGenerator(configuration);
            solutionGenerator.GenerateAll();

            Assert.That(File.Exists(configuration.ConfigurationPath), Is.True, "SolutionGenerator solution generation completed...");
            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "SolutionGenerator solution generation completed...");
        }
    }
}
