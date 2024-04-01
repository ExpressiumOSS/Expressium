using NUnit.Framework;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.CodeGenerators.CSharp;

namespace Expressium.UnitTests.CodeGenerators.CSharp
{
    [TestFixture]
    public class CodeGeneratorTestCSharpTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorTestCSharp codeGeneratorTestCSharp;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorTestCSharp = new CodeGeneratorTestCSharp(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(40), "CodeGeneratorTestCSharp GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateUsings()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateUsings(page);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorTestCSharp GenerateUsings validation");
            Assert.That(listOfLines[1], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorTestCSharp GenerateUsings validation");
            Assert.That(listOfLines[2], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Pages;"), "CodeGeneratorTestCSharp GenerateUsings validation");
            Assert.That(listOfLines[3], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Tests.Factories;"), "CodeGeneratorTestCSharp GenerateUsings validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateNameSpace()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateNameSpace();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestCSharp GenerateNameSpace validation");
            Assert.That(listOfLines[0], Is.EqualTo("namespace Expressium.Coffeeshop.Web.API.Tests.UITests"), "CodeGeneratorTestCSharp GenerateNameSpace validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateTagProperties()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateTagProperties(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestCSharp GenerateTagProperties validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateClass()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestCSharp GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class LoginPageTests : BaseTest"), "CodeGeneratorTestCSharp GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GeneratedDataVariables()
        {
            var listOfLines = codeGeneratorTestCSharp.GeneratedDataVariables(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestCSharp GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestCSharp GeneratedTestDataVariables validation");
            Assert.That(listOfLines[1], Is.EqualTo("private LoginPageModel loginPageModel = LoginPageModelFactory.Default();"), "CodeGeneratorTestCSharp GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GeneratedDataVariables_As_Varibles()
        {
            page.Model = false;
            var listOfLines = codeGeneratorTestCSharp.GeneratedDataVariables(page);
            page.Model = true;

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorTestCSharp GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestCSharp GeneratedDataVariables validation");
            Assert.That(listOfLines[2], Is.EqualTo("private string username = \"Andersen\";"), "CodeGeneratorTestCSharp GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateOneTimeSetUpMethod()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateOneTimeSetUpMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(11), "CodeGeneratorTestCSharp GenerateOneTimeSetUpMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("InitializeBrowser();"), "CodeGeneratorTestCSharp GenerateOneTimeSetUpMethod validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateTitleTestMethod()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateTitleTestMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorTestCSharp GenerateTitleTestMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Asserts.EqualTo(loginPage.GetTitle(), \"Login\", \"Validating the LoginPage Title...\");"), "CodeGeneratorTestCSharp GenerateTitleTestMethod validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GenerateFillFormTestMethods()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateFillFormTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorTestCSharp GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("public void Validate_Page_Property_Username()"), "CodeGeneratorTestCSharp GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[4].Contains("EqualTo(loginPage.GetUsername(), loginPageModel.Username"), "CodeGeneratorTestCSharp GenerateFillFormTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GeneratePageTableTestMethods()
        {
            var listOfLines = codeGeneratorTestCSharp.GenerateTableTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorTestCSharp GenerateTableTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_Recursive_GetNavigationMethods()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.Title = "Login";
            loginPage.AddControl(new ObjectRepositoryControl() { Name = "Login", Target = loginPage.Name });
            objectRepository.AddPage(loginPage);

            var listOfLines = codeGeneratorTestCSharp.GetNavigationMethods(objectRepository, loginPage.Name);

            Assert.That(listOfLines, Is.Null, "CodeGeneratorTestCSharp GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_Deep_Recursive_GetNavigationMethods()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.AddControl(new ObjectRepositoryControl() { Name = "Registration", Target = "RegistrationPage" });
            objectRepository.AddPage(loginPage);

            var registrationPage = new ObjectRepositoryPage();
            registrationPage.Name = "RegistrationPage";
            registrationPage.AddControl(new ObjectRepositoryControl() { Name = "Login", Target = "LoginPage" });
            objectRepository.AddPage(registrationPage);

            var listOfLines = codeGeneratorTestCSharp.GetNavigationMethods(objectRepository, loginPage.Name);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var registrationPage = new RegistrationPage(logger, driver);"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("registrationPage.ClickLogin();"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestCSharp_GetNavigationMethods()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.Title = "Login";
            loginPage.AddControl(new ObjectRepositoryControl() { Name = "Registration", Target = "RegistrationPage" });
            objectRepository.AddPage(loginPage);

            var registrationPage = new ObjectRepositoryPage();
            registrationPage.Name = "RegistrationPage";
            registrationPage.AddControl(new ObjectRepositoryControl() { Name = "Settings", Target = "SettingsPage" });
            objectRepository.AddPage(registrationPage);

            var settingsPage = new ObjectRepositoryPage();
            settingsPage.Name = "SettingsPage";
            objectRepository.AddPage(settingsPage);

            var listOfLines = codeGeneratorTestCSharp.GetNavigationMethods(objectRepository, settingsPage.Name);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var loginPage = new LoginPage(logger, driver);"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("loginPage.ClickRegistration();"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("var registrationPage = new RegistrationPage(logger, driver);"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
            Assert.That(listOfLines[4], Is.EqualTo("registrationPage.ClickSettings();"), "CodeGeneratorTestCSharp GetNavigationMethods validation");
        }

        private static ObjectRepositoryPage CreateLoginPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "LoginPage";
            page.Base = "FramePage";
            page.Title = "Login";
            page.Model = true;

            var synchronizer = new ObjectRepositorySynchronizer() { How = "WaitForPageTitleEquals", Using = "Home" };
            page.AddSynchronizer(synchronizer);

            var member = new ObjectRepositoryMember() { Name = "Menu", Page = "MainMenuBar" };
            page.AddMember(member);

            var username = new ObjectRepositoryControl();
            username.Name = "Username";
            username.Type = "TextBox";
            username.How = "Id";
            username.Using = "username";
            username.Value = "Andersen";
            page.AddControl(username);

            return page;
        }
    }
}
