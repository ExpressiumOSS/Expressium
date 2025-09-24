using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using System;

namespace Expressium.CodeGenerators.UnitTests
{
    [TestFixture]
    public class CodeGeneratorLoadersTests
    {
        [Test]
        public void CodeGeneratorLoaders_GetCodeGenerator()
        {
            var configuration = GetConfiguration();
            var codeGenerator = CodeGeneratorLoaders.GetCodeGenerator(configuration, new ObjectRepository());
            Assert.That(codeGenerator.GetCodingLanguages().Count, Is.EqualTo(1), "CodeGeneratorLoaders GetCodeGenerator validation");
        }

        [Test]
        public void CodeGeneratorLoaders_GetCodeGenerator_GetCodingLanguages()
        {
            var configuration = GetConfiguration();
            Assert.That(CodeGeneratorLoaders.GetCodeGeneratorsCodingLanguages().Count, Is.EqualTo(1), "CodeGeneratorLoaders GetCodeGeneratorsCodingLanguages validation");
        }

        [Test]
        public void CodeGeneratorLoaders_GetCodeGenerator_GetCodingFlavours()
        {
            var configuration = GetConfiguration();
            Assert.That(CodeGeneratorLoaders.GetCodeGeneratorsCodingFlavours(configuration.CodeGenerator.CodingLanguage).Count, Is.EqualTo(2), "CodeGeneratorLoaders GetCodeGeneratorsCodingFlavours validation");
        }

        private Configuration GetConfiguration()
        {
            var configuration = new Configuration();
            configuration.Company = "Microsoft";
            configuration.Project = "Coffeeshop"; ;
            configuration.ApplicationUrl = "http://www.google.com";
            configuration.SolutionPath = Environment.GetEnvironmentVariable("TEMP");
            configuration.CodeGenerator.CodingLanguage = "CSharp";
            configuration.CodeGenerator.CodingFlavour = "Selenium";

            return configuration;
        }
    }
}
