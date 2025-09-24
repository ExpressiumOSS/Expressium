using NUnit.Framework;
using System;
using System.IO;

namespace Expressium.Configurations.UnitTests
{
    [TestFixture]
    public class ConfigurationTests
    {
        string directory = null;

        Configuration configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            directory = Environment.GetEnvironmentVariable("TEMP");
        }

        [Test]
        public void Configuration_SerializeAsJson()
        {
            configuration = CreateConfiguration();

            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            ConfigurationUtilities.SerializeAsJson(configuration.RepositoryPath, configuration);

            Assert.That(File.Exists(configuration.RepositoryPath), Is.True, "Configuration Serialize as JSON validation");
        }

        [Test]
        public void Configuration_DeserializeAsJson()
        {
            configuration = CreateConfiguration();

            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            ConfigurationUtilities.SerializeAsJson(configuration.RepositoryPath, configuration);
            configuration = ConfigurationUtilities.DeserializeAsJson<Configuration>(configuration.RepositoryPath);

            Assert.That("Microsoft", Is.EqualTo(configuration.Company), "Configuration Deserialize as JSON validation");
        }

        [Test]
        public void Configuration_Validate_Undefined_CodingLanguage()
        {
            configuration = CreateConfiguration();

            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            ConfigurationUtilities.SerializeAsJson(configuration.RepositoryPath, configuration);
            Assert.DoesNotThrow(() => configuration.Validate(), "Configuration Validate validate valid configuration");

            //configuration.CodeGenerator.CodingLanguage = "English";
            //var exception = Assert.Throws<ArgumentException>(() => configuration.Validate());
            //Assert.That(exception.Message, Is.EqualTo("The Configuration property 'CodingLanguage' is invalid..."), "Configuration Validate invalid property CodingLanguage");

            configuration.CodeGenerator.CodingLanguage = null;
            var exception = Assert.Throws<ArgumentException>(() => configuration.Validate());
            Assert.That(exception.Message, Is.EqualTo("The ConfigurationCodeGenerator property 'CodingLanguage' is undefined..."), "Configuration Validate invalid property CodingLanguage");
        }

        private Configuration CreateConfiguration()
        {
            var configuration = new Configuration();

            configuration.Company = "Microsoft";
            configuration.Project = "Coffeeshop"; ;
            configuration.ApplicationUrl = "http://www.google.com";
            configuration.SolutionPath = directory;
            configuration.CodeGenerator.CodingLanguage = "CSharp";
            configuration.CodeGenerator.CodingFlavour = "Selenium";

            return configuration;
        }
    }
}