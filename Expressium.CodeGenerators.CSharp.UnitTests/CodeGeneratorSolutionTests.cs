using Expressium.Configurations;
using NUnit.Framework;
using System;
using System.IO;

namespace Expressium.CodeGenerators.CSharp.UnitTests
{
    [TestFixture]
    public class CodeGeneratorSolutionTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        public void CodeGeneratorSolution_GenerateAll()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            configuration.ApplicationUrl = "http://www.dr.dk";
            configuration.SolutionPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "CodeGeneratorSolution");
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.CSharp.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Specflow.ToString();
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            Directory.CreateDirectory(configuration.SolutionPath);

            var codeGeneratorSolution = new CodeGeneratorSolution(configuration);
            codeGeneratorSolution.GenerateAll();

            Assert.That(File.Exists(configuration.ConfigurationPath), Is.True, "CodeGeneratorSolution solution generation completed...");
            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "CodeGeneratorSolution solution generation completed...");
        }
    }
}
