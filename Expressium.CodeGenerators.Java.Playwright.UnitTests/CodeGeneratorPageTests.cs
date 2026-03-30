using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Playwright.UnitTests
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
        public void CodeGeneratorPageJavaPlaywright_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPage.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(39), "CodeGeneratorPageJavaPlaywright GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GeneratePackage()
        {
            var listOfLines = codeGeneratorPage.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJavaPlaywright GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.pages;"), "CodeGeneratorPageJavaPlaywright GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateImports()
        {
            var listOfLines = codeGeneratorPage.GenerateImports(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorPageJavaPlaywright GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import expressium.coffeeshop.web.api.models.*;"), "CodeGeneratorPageJavaPlaywright GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateClass()
        {
            var listOfLines = codeGeneratorPage.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJavaPlaywright GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class LoginPage extends FramePage"), "CodeGeneratorPageJavaPlaywright GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateMembers()
        {
            var listOfLines = codeGeneratorPage.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJavaPlaywright GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar menu;"), "CodeGeneratorPageJavaPlaywright GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateMembers_With_Table()
        {
            var tablePage = page.Copy();
            tablePage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPage.GenerateMembers(tablePage);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJavaPlaywright GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar menu;"), "CodeGeneratorPageJavaPlaywright GenerateMembers validation");
            Assert.That(listOfLines[1], Is.EqualTo("public BaseTable grid;"), "CodeGeneratorPageJavaPlaywright GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateLocators()
        {
            var listOfLines = codeGeneratorPage.GenerateLocators(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJavaPlaywright GenerateLocators validation");
            Assert.That(listOfLines[0], Is.EqualTo("private WebTextBox username;"), "CodeGeneratorPageJavaPlaywright GenerateLocators validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateConstructor()
        {
            var listOfLines = codeGeneratorPage.GenerateContructor(page);

            Assert.That(listOfLines.Count, Is.EqualTo(8), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(Logger logger, Page page) {"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("this.menu = new MainMenuBar(logger, page);"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[5], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateConstructor_With_Table()
        {
            var tablePage = page.Copy();
            tablePage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPage.GenerateContructor(tablePage);

            Assert.That(listOfLines.Count, Is.EqualTo(9), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(Logger logger, Page page) {"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("this.menu = new MainMenuBar(logger, page);"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[3], Is.EqualTo("this.grid = new BaseTable(logger, page, page.locator(\"[id='products']\"));"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
            Assert.That(listOfLines[6], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJavaPlaywright GenerateConstructor validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateActionMethods()
        {
            var listOfLines = codeGeneratorPage.GenerateActionMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(10), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setUsername(String value) {"), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
            Assert.That(listOfLines[1], Is.EqualTo("logger.info(\"setUsername(\" + value + \")\");"), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("username.setText(value);"), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
            Assert.That(listOfLines[5], Is.EqualTo("public String getUsername() {"), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
            Assert.That(listOfLines[7], Is.EqualTo("return username.getText();"), "CodeGeneratorPageJavaPlaywright GenerateActionMethods validation");
        }

        [Test]
        public void CodeGeneratorPageJavaPlaywright_GenerateFillFormMethod()
        {
            var listOfLines = codeGeneratorPage.GenerateFillFormMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorPageJavaPlaywright GenerateFillFormMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void fillForm(LoginPageModel model) {"), "CodeGeneratorPageJavaPlaywright GenerateFillFormMethod validation");
            Assert.That(listOfLines[1], Is.EqualTo("setUsername(model.getUsername());"), "CodeGeneratorPageJavaPlaywright GenerateFillFormMethod validation");
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
