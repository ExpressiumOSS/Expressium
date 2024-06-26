using NUnit.Framework;
using NUnit.Framework.Internal;
using Expressium.CodeGenerators.Java;
using Expressium.Configurations;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.CodeGenerators.Java
{
    [TestFixture]
    public class CodeGeneratorPageJavaTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorPageJava codeGeneratorPageJava;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPageJava = new CodeGeneratorPageJava(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPageJava.GenerateSourceCode(page);

            if (configuration.IsCodingStyleByLocators())
                Assert.That(listOfLines.Count, Is.EqualTo(43), "CodeGeneratorPageJava GenerateSourceCode validation");
            else
                Assert.That(listOfLines.Count, Is.EqualTo(54), "CodeGeneratorPageJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateImports()
        {
            var listOfLines = codeGeneratorPageJava.GenerateImports(page);

            if (configuration.IsCodingStyleByLocators())
            {
                Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateImports validation");
                Assert.That(listOfLines[1], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageJava GenerateImports validation");
            }
            else
            {
                Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateImports validation");
                Assert.That(listOfLines[2], Is.EqualTo("import Bases.BasePage;"), "CodeGeneratorPageJava GenerateImports validation");
            }
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMembers()
        {
            var listOfLines = codeGeneratorPageJava.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJava GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("private MainMenuBar Menu;"), "CodeGeneratorPageJava GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateLocators()
        {
            var listOfLines = codeGeneratorPageJava.GenerateLocators(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJava GenerateLocators validation");
            Assert.That(listOfLines[0], Is.EqualTo("@FindBy(how = How.ID, using = \"username\")"), "CodeGeneratorPageJava GenerateLocators validation");
            Assert.That(listOfLines[1], Is.EqualTo("private WebElement username;"), "CodeGeneratorPageJava GenerateLocators validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateContructor()
        {
            var listOfLines = codeGeneratorPageJava.GenerateContructor(page);

            Assert.That(listOfLines.Count, Is.EqualTo(9), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(WebDriver driver) throws Exception"), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[4], Is.EqualTo("Menu = new MainMenuBar(driver);"), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[6], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJava GenerateContructor validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMemberMethods()
        {
            var listOfLines = codeGeneratorPageJava.GenerateMemberMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorPageJava GenerateMemberMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar getMainMenuBar()"), "CodeGeneratorPageJava GenerateMemberMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("return Menu;"), "CodeGeneratorPageJava GenerateMemberMethods validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateControlMethods()
        {
            var listOfLines = codeGeneratorPageJava.GenerateControlMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateControlMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setUsername(String value) throws Exception"), "CodeGeneratorPageJava GenerateControlMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setTextBox(driver, username, value);"), "CodeGeneratorPageJava GenerateControlMethods validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getUsername() throws Exception"), "CodeGeneratorPageJava GenerateControlMethods validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getTextBox(driver, username);"), "CodeGeneratorPageJava GenerateControlMethods validation");
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
