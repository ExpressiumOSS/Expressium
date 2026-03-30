using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Selenium.UnitTests
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
        public void CodeGeneratorTestJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorTest.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(36), "CodeGeneratorTestJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GeneratePackage()
        {
            var listOfLines = codeGeneratorTest.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestJava GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.uitests;"), "CodeGeneratorTestJava GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateImports()
        {
            var listOfLines = codeGeneratorTest.GenerateImports(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorTestJava GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import expressium.coffeeshop.web.api.models.*;"), "CodeGeneratorTestJava GenerateImports validation");
            Assert.That(listOfLines[3], Is.EqualTo("import expressium.coffeeshop.web.api.pages.*;"), "CodeGeneratorTestJava GenerateImports validation");
            Assert.That(listOfLines[4], Is.EqualTo("import expressium.coffeeshop.web.api.factories.*;"), "CodeGeneratorTestJava GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateTagProperties()
        {
            var listOfLines = codeGeneratorTest.GenerateTagProperties(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJava GenerateTagProperties validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateClass()
        {
            var listOfLines = codeGeneratorTest.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorTestJava GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class LoginPageTests extends BaseTest"), "CodeGeneratorTestJava GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GeneratedDataVariables()
        {
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJava GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestJava GeneratedDataVariables validation");
            Assert.That(listOfLines[1], Is.EqualTo("private LoginPageModel loginPageModel = LoginPageModelFactory.getDefault();"), "CodeGeneratorTestJava GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GeneratedDataVariables_As_Variables()
        {
            page.Model = false;
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);
            page.Model = true;

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorTestJava GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorTestJava GeneratedDataVariables validation");
            Assert.That(listOfLines[2], Is.EqualTo("private String username = \"Andersen\";"), "CodeGeneratorTestJava GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateSetUpMethod()
        {
            var listOfLines = codeGeneratorTest.GenerateSetUpMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(10), "CodeGeneratorTestJava GenerateSetUpMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("initializeBrowser();"), "CodeGeneratorTestJava GenerateSetUpMethod validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateTitleTestMethod()
        {
            var listOfLines = codeGeneratorTest.GenerateTitleTestMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorTestJava GenerateTitleTestMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("asserts.equalTo(loginPage.getTitle(), \"Login\", \"Validating the LoginPage Title...\");"), "CodeGeneratorTestJava GenerateTitleTestMethod validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateFillFormTestMethods()
        {
            var listOfLines = codeGeneratorTest.GenerateFillFormTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorTestJava GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("public void validate_Page_Property_Username() {"), "CodeGeneratorTestJava GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[3].Contains("equalTo(loginPage.getUsername(), loginPageModel.getUsername()"), Is.True, "CodeGeneratorTestJava GenerateFillFormTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GenerateTableTestMethods()
        {
            var listOfLines = codeGeneratorTest.GenerateTableTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorTestJava GenerateTableTestMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJava_Recursive_GetNavigationMethods()
        {
            var objectRepository = new ObjectRepository();

            var loginPage = new ObjectRepositoryPage();
            loginPage.Name = "LoginPage";
            loginPage.Title = "Login";
            loginPage.AddControl(new ObjectRepositoryControl() { Name = "Login", Target = loginPage.Name });
            objectRepository.AddPage(loginPage);

            var listOfLines = codeGeneratorTest.GetNavigationMethods(objectRepository, loginPage.Name);

            Assert.That(listOfLines, Is.Null, "CodeGeneratorTestJava GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJava_Deep_Recursive_GetNavigationMethods()
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

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var registrationPage = new RegistrationPage(logger, driver);"), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("registrationPage.clickLogin();"), "CodeGeneratorTestJava GetNavigationMethods validation");
        }

        [Test]
        public void CodeGeneratorTestJava_GetNavigationMethods()
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

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("var loginPage = new LoginPage(logger, driver);"), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("loginPage.clickRegistration();"), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("var registrationPage = new RegistrationPage(logger, driver);"), "CodeGeneratorTestJava GetNavigationMethods validation");
            Assert.That(listOfLines[4], Is.EqualTo("registrationPage.clickSettings();"), "CodeGeneratorTestJava GetNavigationMethods validation");
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
