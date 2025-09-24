using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using System;
using System.IO;

namespace Expressium.CodeGenerators.CSharp.Selenium.UnitTests
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        string directory = null;

        Configuration configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            directory = Environment.GetEnvironmentVariable("TEMP");

            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            configuration.ApplicationUrl = "http://www.google.com";
            configuration.SolutionPath = directory;
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.CSharp.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Selenium.ToString();
        }

        [Test]
        public void CodeGenerator_GenerateAll()
        {
            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            var loginPageFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API", "Pages", "LoginPage.cs");
            if (File.Exists(loginPageFile))
                File.Delete(loginPageFile);

            var loginModelFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API", "Models", "LoginPageModel.cs");
            if (File.Exists(loginModelFile))
                File.Delete(loginModelFile);

            var loginTestFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API.Tests", "UITests", "LoginPageTests.cs");
            if (File.Exists(loginTestFile))
                File.Delete(loginTestFile);

            var loginFactoryFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API.Tests", "Factories", "LoginPageModelFactory.cs");
            if (File.Exists(loginFactoryFile))
                File.Delete(loginFactoryFile);

            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, objectRepository);

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            codeGenerator.GenerateAll();

            Assert.That(File.Exists(loginPageFile), Is.True, "CodeGenerator GenerateAll validation");
            Assert.That(File.Exists(loginModelFile), Is.True, "CodeGenerator GenerateAll validation");
            Assert.That(File.Exists(loginTestFile), Is.True, "CodeGenerator GenerateAll validation");
            Assert.That(File.Exists(loginFactoryFile), Is.True, "CodeGenerator GenerateAll validation");
        }

        [Test]
        public void CodeGenerator_GeneratePage()
        {
            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            var loginPageFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API", "Pages", "LoginPage.cs");
            if (File.Exists(loginPageFile))
                File.Delete(loginPageFile);

            var loginPageModelFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API", "Models", "LoginPageModel.cs");
            if (File.Exists(loginPageModelFile))
                File.Delete(loginPageModelFile);

            var loginTestFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API.Tests", "UITests", "LoginPageTests.cs");
            if (File.Exists(loginTestFile))
                File.Delete(loginTestFile);

            var loginFactoryFile = Path.Combine(directory, "Expressium.Coffeeshop.Web.API.Tests", "Factories", "LoginPageModelFactory.cs");
            if (File.Exists(loginFactoryFile))
                File.Delete(loginFactoryFile);

            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, objectRepository);

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            codeGenerator.GeneratePage("LoginPage");

            Assert.That(File.Exists(loginPageFile), Is.True, "CodeGenerator GeneratePage validation");
            Assert.That(File.Exists(loginPageModelFile), Is.True, "CodeGenerator GeneratePage validation");
            Assert.That(File.Exists(loginTestFile), Is.True, "CodeGenerator GeneratePage validation");
            Assert.That(File.Exists(loginFactoryFile), Is.True, "CodeGenerator GeneratePage validation");
        }

        [Test]
        public void CodeGenerator_GeneratePagePreview()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GeneratePagePreview("LoginPage");

            Assert.That(result, Does.Contain("LoginPage"), "CodeGenerator GeneratePagePreview validation");
        }

        [Test]
        public void CodeGenerator_GenerateModelPreview()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateModelPreview("LoginPage");

            Assert.That(result, Does.Contain("LoginPageModel"), "CodeGenerator GenerateModelPreview validation");
        }

        [Test]
        public void CodeGenerator_GenerateTestPreview()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateTestPreview("LoginPage");

            Assert.That(result, Does.Contain("LoginPageModel"), "CodeGenerator GenerateTestPreview validation");
        }

        [Test]
        public void CodeGenerator_GenerateFactoryPreview()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateFactoryPreview("LoginPage");

            Assert.That(result, Does.Contain("LoginPageModelFactory"), "CodeGenerator GenerateFactoryPreview validation");
        }

        private ObjectRepositoryPage CreateLoginPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "LoginPage";

            var username = new ObjectRepositoryControl();
            username.Name = "Username";
            username.Type = "TextBox";
            username.How = "Id";
            username.Using = "username";
            page.AddControl(username);

            var password = new ObjectRepositoryControl();
            password.Name = "Password";
            password.Type = "TextBox";
            password.How = "Name";
            password.Using = "password";
            page.AddControl(password);

            var submit = new ObjectRepositoryControl();
            submit.Name = "LogIn";
            submit.Type = "Button";
            submit.How = "XPath";
            submit.Using = "//button[text()='LogIn']";
            page.AddControl(submit);

            page.Model = true;

            return page;
        }
    }
}
