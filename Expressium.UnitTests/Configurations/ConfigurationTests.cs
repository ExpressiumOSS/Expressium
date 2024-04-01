using NUnit.Framework;
using Expressium.Configurations;
using System;
using System.IO;

namespace Expressium.UnitTests.Configurations
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
        public void Configuration_GetNameSpace()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            var expected = "Expressium.Coffeeshop.Web.API";

            var result = configuration.GetNameSpace();
            Assert.That(result, Is.EqualTo(expected), "Configuration GetNameSpace validate generated output");
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
        public void Configuration_Validate_Invalid_CodingLanguage()
        {
            configuration = CreateConfiguration();

            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            ConfigurationUtilities.SerializeAsJson(configuration.RepositoryPath, configuration);
            Assert.DoesNotThrow(() => configuration.Validate(), "Configuration Validate validate valid configuration");

            configuration.CodingLanguage = "English";
            var exception = Assert.Throws<ArgumentException>(() => configuration.Validate());
            Assert.That(exception.Message, Is.EqualTo("The Configuration property 'CodingLanguage' is invalid..."), "Configuration Validate invalid property CodingLanguage");

            configuration.CodingLanguage = null;
            exception = Assert.Throws<ArgumentException>(() => configuration.Validate());
            Assert.That(exception.Message, Is.EqualTo("The Configuration property 'CodingLanguage' is invalid..."), "Configuration Validate invalid property CodingLanguage");
        }

        private Configuration CreateConfiguration()
        {
            var configuration = new Configuration();

            configuration.Company = "Microsoft";
            configuration.Project = "Coffeeshop"; ;
            configuration.ApplicationUrl = "http://www.google.com";
            configuration.SolutionPath = directory;
            configuration.CodingLanguage = CodingLanguages.CSharp.ToString();
            configuration.CodingFlavour = CodingFlavours.Specflow.ToString();
            configuration.CodingStyle = CodingStyles.PageFactory.ToString();

            return configuration;
        }
    }
}
