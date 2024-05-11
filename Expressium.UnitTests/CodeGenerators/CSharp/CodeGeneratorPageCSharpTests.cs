using Expressium.CodeGenerators.CSharp;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Expressium.UnitTests.CodeGenerators.CSharp
{
    [TestFixture]
    public class CodeGeneratorPageCSharpTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorPageCSharp codeGeneratorPageCSharp;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";
            //configuration.CodeGenerator.CodingStyle = CodingStyles.ByLocators.ToString();

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPageCSharp = new CodeGeneratorPageCSharp(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateSourceCode(page);

            if (configuration.IsCodingStyleByLocators())
                Assert.That(listOfLines.Count, Is.EqualTo(43), "CodeGeneratorPageCSharp GenerateSourceCode validation");
            else
                Assert.That(listOfLines.Count, Is.EqualTo(45), "CodeGeneratorPageCSharp GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateUsings()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateUsings(page);

            if (configuration.IsCodingStyleByLocators())
            {
                Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateUsings validation");
                Assert.That(listOfLines[1], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageCSharp GenerateUsings validation");
            }
            else
            {
                Assert.That(listOfLines.Count, Is.EqualTo(7), "CodeGeneratorPageCSharp GenerateUsings validation");
                Assert.That(listOfLines[2], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageCSharp GenerateUsings validation");
            }
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateNameSpace()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateNameSpace();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateNameSpace validation");
            Assert.That(listOfLines[0], Is.EqualTo("namespace Expressium.Coffeeshop.Web.API.Pages"), "CodeGeneratorPageCSharp GenerateNameSpace validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateClass()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public partial class LoginPage : FramePage"), "CodeGeneratorPageCSharp GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMembers()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar Menu { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMembers_With_Table()
        {
            var tabelPage = page.Copy();
            tabelPage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPageCSharp.GenerateMembers(tabelPage);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar Menu { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[1], Is.EqualTo("public BaseTable Grid { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateLocators()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateLocators(page);

            if (configuration.IsCodingStyleByLocators())
                Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageCSharp GenerateLocators validation");
            else
            {
                Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageCSharp GenerateLocators validation");
                Assert.That(listOfLines[0], Is.EqualTo("[FindsBy(How = How.Id, Using = \"username\")]"), "CodeGeneratorPageCSharp GenerateLocators validation");
                Assert.That(listOfLines[1], Is.EqualTo("private IWebElement Username { get; set; }"), "CodeGeneratorPageCSharp GenerateLocators validation");
            }
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateContructor()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateContructor(page);

            Assert.That(listOfLines.Count, Is.EqualTo(7), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(ILog logger, IWebDriver driver) : base(logger, driver)"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("Menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[4], Is.EqualTo("WaitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageCSharp GenerateContructor validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateContructor_With_Table()
        {
            var tabelPage = page.Copy();
            tabelPage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPageCSharp.GenerateContructor(tabelPage);

            Assert.That(listOfLines.Count, Is.EqualTo(8), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(ILog logger, IWebDriver driver) : base(logger, driver)"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("Menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[3], Is.EqualTo("Grid = new BaseTable(logger, driver, By.Id(\"products\"));"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[5], Is.EqualTo("WaitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageCSharp GenerateContructor validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateControlMethods()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateControlMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetUsername(string value)"), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("logger.InfoFormat(\"SetUsername({0})\", value);"), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("Username.SetTextBox(driver, value);"), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetUsername()"), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[8], Is.EqualTo("logger.InfoFormat(\"GetUsername()\");"), "CodeGeneratorPageCSharp GenerateControlMethods validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Username.GetTextBox(driver);"), "CodeGeneratorPageCSharp GenGenerateControlMethodserateMethods validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateFillFormMethod()
        {
            var listOfLines = codeGeneratorPageCSharp.GenerateFillFormMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void FillForm(LoginPageModel model)"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("SetUsername(model.Username);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
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
