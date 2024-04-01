using NUnit.Framework;
using System;
using System.IO;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.ObjectRepositories
{
    [TestFixture]
    public class ObjectRepositoryUtilitiesTests
    {
        string fileName = null;
        string directory = null;

        [OneTimeSetUp]
        public void Setup()
        {
            directory = Environment.GetEnvironmentVariable("TEMP");
            fileName = directory + "\\ObjectRepository.json";
        }

        [Test]
        public void ObjectRepository_SerializeAsJson()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var objectRepository = CreateObjectRepository();

            ObjectRepositoryUtilities.SerializeAsJson(fileName, objectRepository);

            Assert.That(File.Exists(fileName), Is.True, "ObjectRepository Serialize as JSON validation");
        }

        [Test]
        public void ObjectRepository_DeserializeAsJson()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var objectRepository = CreateObjectRepository();

            ObjectRepositoryUtilities.SerializeAsJson(fileName, objectRepository);
            objectRepository = ObjectRepositoryUtilities.DeserializeAsJson<ObjectRepository>(fileName);

            Assert.That(2, Is.EqualTo(objectRepository.Pages.Count), "ObjectRepository Deserialize pages as XML validation");
        }

        [Test]
        public void ObjectRepository_Validate()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var objectRepository = CreateObjectRepository();
            Assert.DoesNotThrow(() => objectRepository.Validate(), "ObjectRepository Validate validate valid objectRepository");
        }

        private ObjectRepository CreateObjectRepository()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.Model = true;
            var username = new ObjectRepositoryControl();
            username.Name = "Username";
            username.Type = "TextBox";
            username.How = "Id";
            username.Using = "username";
            loginPage.AddControl(username);
            objectRepository.AddPage(loginPage);

            var mainMenuBar = new ObjectRepositoryPage();
            mainMenuBar.Name = "MainMenuBar";
            var home = new ObjectRepositoryControl();
            home.Name = "Home";
            home.Type = "Link";
            home.How = "XPath";
            home.Using = "//a[text()='Home']";
            mainMenuBar.AddControl(home);
            objectRepository.AddPage(mainMenuBar);

            return objectRepository;
        }
    }
}
