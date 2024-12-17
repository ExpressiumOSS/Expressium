using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Expressium.CodeGenerators.Java.UnitTests
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
            configuration.CodeGenerator.CodingStyle = CodingStyles.PageFactory.ToString();

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPage = new CodeGeneratorPage(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPage.GenerateSourceCode(page);

            //if (IsCodingStyleByLocators())
            //    Assert.That(listOfLines.Count, Is.EqualTo(43), "CodeGeneratorPageJava GenerateSourceCode validation");
            //else
                Assert.That(listOfLines.Count, Is.EqualTo(54), "CodeGeneratorPageJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateImports()
        {
            var listOfLines = codeGeneratorPage.GenerateImports(page);

            //if (IsCodingStyleByLocators())
            //{
            //    Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateImports validation");
            //    Assert.That(listOfLines[1], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageJava GenerateImports validation");
            //}
            //else
            {
                Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateImports validation");
                Assert.That(listOfLines[2], Is.EqualTo("import Bases.*;"), "CodeGeneratorPageJava GenerateImports validation");
            }
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMembers()
        {
            var listOfLines = codeGeneratorPage.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJava GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("private MainMenuBar Menu;"), "CodeGeneratorPageJava GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateLocators()
        {
            var listOfLines = codeGeneratorPage.GenerateLocators(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJava GenerateLocators validation");
            Assert.That(listOfLines[0], Is.EqualTo("@FindBy(how = How.ID, using = \"username\")"), "CodeGeneratorPageJava GenerateLocators validation");
            Assert.That(listOfLines[1], Is.EqualTo("private WebElement username;"), "CodeGeneratorPageJava GenerateLocators validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateContructor()
        {
            var listOfLines = codeGeneratorPage.GenerateContructor(page);

            Assert.That(listOfLines.Count, Is.EqualTo(9), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(Logger logger, WebDriver driver) throws Exception"), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[4], Is.EqualTo("Menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageJava GenerateContructor validation");
            Assert.That(listOfLines[6], Is.EqualTo("waitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageJava GenerateContructor validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMemberMethods()
        {
            var listOfLines = codeGeneratorPage.GenerateMemberMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorPageJava GenerateMemberMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar getMainMenuBar()"), "CodeGeneratorPageJava GenerateMemberMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("return Menu;"), "CodeGeneratorPageJava GenerateMemberMethods validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethods()
        {
            var listOfLines = codeGeneratorPage.GenerateActionMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setUsername(String value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setTextBox(driver, username, value);"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getUsername() throws Exception"), "CodeGeneratorPageJava GenerateActionMethods validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getTextBox(driver, username);"), "CodeGeneratorPageJava GenerateActionMethods validation");
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
