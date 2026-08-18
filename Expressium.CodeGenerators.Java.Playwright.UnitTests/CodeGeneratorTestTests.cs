using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Playwright.UnitTests
{
    [TestFixture]
    public class CodeGeneratorTestTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorTest codeGeneratorTest;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorTest = new CodeGeneratorTest(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorTest.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(36), "CodeGeneratorTestJavaPlaywright GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GeneratePackage()
        {
            var listOfLines = codeGeneratorTest.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestJavaPlaywright GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.uitests;"), "CodeGeneratorTestJavaPlaywright GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateImports()
        {
            var listOfLines = codeGeneratorTest.GenerateImports(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorTestJavaPlaywright GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import expressium.coffeeshop.web.api.models.*;"), "CodeGeneratorTestJavaPlaywright GenerateImports validation");
            Assert.That(listOfLines[3], Is.EqualTo("import expressium.coffeeshop.web.api.pages.*;"), "CodeGeneratorTestJavaPlaywright GenerateImports validation");
            Assert.That(listOfLines[4], Is.EqualTo("import expressium.coffeeshop.web.api.factories.*;"), "CodeGeneratorTestJavaPlaywright GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateTagProperties()
        {
            var listOfLines = codeGeneratorTest.GenerateTagProperties(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJavaPlaywright GenerateTagProperties validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateClass()
        {
            var listOfLines = codeGeneratorTest.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestJavaPlaywright GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class LoginPageTests extends BaseTest"), "CodeGeneratorTestJavaPlaywright GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GeneratedDataVariables()
        {
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
            Assert.That(listOfLines[1], Is.EqualTo("private LoginPageModel loginPageModel = LoginPageModelFactory.getDefault();"), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GeneratedDataVariables_As_Variables()
        {
            page.Model = false;
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);
            page.Model = true;

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
            Assert.That(listOfLines[2], Is.EqualTo("private String username = \"Andersen\";"), "CodeGeneratorTestJavaPlaywright GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateSetUpMethod()
        {
            var listOfLines = codeGeneratorTest.GenerateSetUpMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(10), "CodeGeneratorTestJavaPlaywright GenerateSetUpMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("initializeBrowser();"), "CodeGeneratorTestJavaPlaywright GenerateSetUpMethod validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateTitleTestMethod()
        {
            var listOfLines = codeGeneratorTest.GenerateTitleTestMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorTestJavaPlaywright GenerateTitleTestMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("asserts.equalTo(loginPage.getTitle(), \"Login\", \"Validating the LoginPage Title...\");"), "CodeGeneratorTestJavaPlaywright GenerateTitleTestMethod validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateFillFormTestMethods()
        {
            var listOfLines = codeGeneratorTest.GenerateFillFormTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorTestJavaPlaywright GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("public void validate_Page_Property_Username() {"), "CodeGeneratorTestJavaPlaywright GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[3].Contains("equalTo(loginPage.getUsername(), loginPageModel.getUsername()"), Is.True, "CodeGeneratorTestJavaPlaywright GenerateFillFormTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GenerateTableTestMethods()
        {
            var listOfLines = codeGeneratorTest.GenerateTableTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorTestJavaPlaywright GenerateTableTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_Recursive_GetNavigationMethods()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.Title = "Login";
            loginPage.AddControl(new ObjectRepositoryControl() { Name = "Login", Target = loginPage.Name });
            objectRepository.AddPage(loginPage);

            var listOfLines = codeGeneratorTest.GetNavigationMethods(objectRepository, loginPage.Name);

            Assert.That(listOfLines, Is.Null, "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_Deep_Recursive_GetNavigationMethods()
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

            var listOfLines = codeGeneratorTest.GetNavigationMethods(objectRepository, loginPage.Name);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var registrationPage = new RegistrationPage(logger, page);"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("registrationPage.clickLogin();"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJavaPlaywright_GetNavigationMethods()
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

            var listOfLines = codeGeneratorTest.GetNavigationMethods(objectRepository, settingsPage.Name);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var loginPage = new LoginPage(logger, page);"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("loginPage.clickRegistration();"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("var registrationPage = new RegistrationPage(logger, page);"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
            Assert.That(listOfLines[4], Is.EqualTo("registrationPage.clickSettings();"), "CodeGeneratorTestJavaPlaywright GetNavigationMethods validation");
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
