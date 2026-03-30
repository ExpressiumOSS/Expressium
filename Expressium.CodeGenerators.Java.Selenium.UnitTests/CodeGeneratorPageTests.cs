using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Selenium.UnitTests
{
    [TestFixture]
    public class CodeGeneratorPageTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorPage codeGeneratorPage;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPage = new CodeGeneratorPage(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPage.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(39), "CodeGeneratorPageJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GeneratePackage()
        {
            var listOfLines = codeGeneratorPage.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJava GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.pages;"), "CodeGeneratorPageJava GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateImports()
        {
            var listOfLines = codeGeneratorPage.GenerateImports(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import expressium.coffeeshop.web.api.models.*;"), "CodeGeneratorPageJava GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateClass()
        {
            var listOfLines = codeGeneratorPage.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJava GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class LoginPage extends FramePage"), "CodeGeneratorPageJava GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMembers()
        {
            var listOfLines = codeGeneratorPage.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJava GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar menu;"), "CodeGeneratorPageJava GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMembers_With_Table()
        {
            var tablePage = page.Copy();
            tablePage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPage.GenerateMembers(tablePage);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJava GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar menu;"), "CodeGeneratorPageJava GenerateMembers validation");
            Assert.That(listOfLines[1], Is.EqualTo("public BaseTable grid;"), "CodeGeneratorPageJava GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateLocators()
        {
            var listOfLines = codeGeneratorPage.GenerateLocators(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJava GenerateLocators validation");
            Assert.That(listOfLines[0], Is.EqualTo("private WebTextBox username;"), "CodeGeneratorPageJava GenerateLocators validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateConstructor()
        {
            var listOfLines = codeGeneratorPage.GenerateContructor(page);

            Assert.That(listOfLines.Count, Is.EqualTo(8), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(Logger logger, WebDriver driver) {"), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("this.menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[5], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJava GenerateConstructor validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateConstructor_With_Table()
        {
            var tablePage = page.Copy();
            tablePage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPage.GenerateContructor(tablePage);

            Assert.That(listOfLines.Count, Is.EqualTo(9), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(Logger logger, WebDriver driver) {"), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("this.menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[3], Is.EqualTo("this.grid = new BaseTable(logger, driver, By.id(\"products\"));"), "CodeGeneratorPageJava GenerateConstructor validation");
            Assert.That(listOfLines[6], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJava GenerateConstructor validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethods()
        {
            var listOfLines = codeGeneratorPage.GenerateActionMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(10), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setUsername(String value) {"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("logger.info(\"setUsername(\" + value + \")\");"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("username.setText(value);"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[5], Is.EqualTo("public String getUsername() {"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[7], Is.EqualTo("return username.getText();"), "CodeGeneratorPageJava GenerateActionMethods validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateFillFormMethod()
        {
            var listOfLines = codeGeneratorPage.GenerateFillFormMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorPageJava GenerateFillFormMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void fillForm(LoginPageModel model) {"), "CodeGeneratorPageJava GenerateFillFormMethod validation");
            Assert.That(listOfLines[1], Is.EqualTo("setUsername(model.getUsername());"), "CodeGeneratorPageJava GenerateFillFormMethod validation");
        }

        private static ObjectRepositoryPage CreateLoginPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "LoginPage";
            page.Base = "FramePage";
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
            page.AddControl(username);

            return page;
        }
    }
}
