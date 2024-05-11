using NUnit.Framework;
using Expressium.CodeGenerators;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;
using System.IO;

namespace Expressium.UnitTests.CodeGenerators.Java
{
    [TestFixture]
    public class CodeGeneratorJavaTests
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
            configuration.CodeGenerator.CodingLanguage = CodingLanguages.Java.ToString();
            configuration.CodeGenerator.CodingFlavour = CodingFlavours.Cucumber.ToString();
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();
        }

        [Test]
        public void CodeGenerator_Java_GenerateAll()
        {
            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            var loginPageFile = Path.Combine(directory, "src\\main\\java", "Pages", "LoginPage.java");
            if (File.Exists(loginPageFile))
                File.Delete(loginPageFile);

            var loginModelFile = Path.Combine(directory, "src\\main\\java", "Models", "LoginPageModel.java");
            if (File.Exists(loginModelFile))
                File.Delete(loginModelFile);

            var loginTestFile = Path.Combine(directory, "src\\test\\java", "UITests", "LoginPageTests.java");
            if (File.Exists(loginTestFile))
                File.Delete(loginTestFile);

            var loginFactoryFile = Path.Combine(directory, "src\\test\\java", "Factories", "LoginPageModelFactory.java");
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
        public void CodeGenerator_Java_GeneratePage()
        {
            if (File.Exists(configuration.RepositoryPath))
                File.Delete(configuration.RepositoryPath);

            var loginPageFile = Path.Combine(directory, "src\\main\\java", "Pages", "LoginPage.java");
            if (File.Exists(loginPageFile))
                File.Delete(loginPageFile);

            var loginPageModelFile = Path.Combine(directory, "src\\main\\java", "Models", "LoginPageModel.java");
            if (File.Exists(loginPageModelFile))
                File.Delete(loginPageModelFile);

            var loginTestFile = Path.Combine(directory, "src\\test\\java", "UITests", "LoginPageTests.java");
            if (File.Exists(loginTestFile))
                File.Delete(loginTestFile);

            var loginFactoryFile = Path.Combine(directory, "src\\test\\java", "Factories", "LoginPageModelFactory.java");
            if (File.Exists(loginFactoryFile))
                File.Delete(loginFactoryFile);

            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            ObjectRepositoryUtilities.SerializeAsJson(configuration.RepositoryPath, objectRepository);

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            codeGenerator.GeneratePage("LoginPage");

            Assert.That(File.Exists(loginPageFile), Is.True, "CodeGenerator GeneratePage validation");
            Assert.That(File.Exists(loginPageModelFile), Is.True, "CodeGenerator GeneratePage validation");
            Assert.That(File.Exists(loginTestFile), Is.True, "CodeGenerator GenerateTest validation");
            Assert.That(File.Exists(loginFactoryFile), Is.True, "CodeGenerator GenerateTest validation");
        }

        [Test]
        public void CodeGenerator_Java_GeneratePageAsString()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GeneratePageAsString("LoginPage");

            Assert.That(result, Does.Contain("LoginPage"), "CodeGenerator GeneratePageAsString validation");
        }

        [Test]
        public void CodeGenerator_Java_GenerateModelAsString()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateModelAsString("LoginPage");

            Assert.That(result, Does.Contain("LoginPageModel"), "CodeGenerator GenerateModelAsString validation");
        }

        [Test]
        public void CodeGenerator_Java_GenerateTestAsString()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateTestAsString("LoginPage");

            Assert.That(result, Does.Contain("LoginPageTests"), "CodeGenerator GenerateTestAsString validation");
        }

        [Test]
        public void CodeGenerator_Java_GenerateFactoryAsString()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            var result = codeGenerator.GenerateFactoryAsString("LoginPage");

            Assert.That(result, Does.Contain("LoginPageModelFactory"), "CodeGenerator GenerateFactoryAsString validation");
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
