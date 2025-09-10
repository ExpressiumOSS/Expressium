using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Expressium.CodeGenerators.CSharp.UnitTests
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
            configuration.CodeGenerator.CodingStyle = CodingStyles.ByControls.ToString();

            page = CreateLoginPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPage = new CodeGeneratorPage(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorPage.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(44), "CodeGeneratorPageCSharp GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateUsings()
        {
            var listOfLines = codeGeneratorPage.GenerateUsings(page);

            //if (configuration.IsCodingStyleByLocators())
            //{
            //    Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateUsings validation");
            //    Assert.That(listOfLines[1], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageCSharp GenerateUsings validation");
            //}
            //else
            {
                Assert.That(listOfLines.Count, Is.EqualTo(7), "CodeGeneratorPageCSharp GenerateUsings validation");
                Assert.That(listOfLines[2], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorPageCSharp GenerateUsings validation");
            }
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateNameSpace()
        {
            var listOfLines = codeGeneratorPage.GenerateNameSpace();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateNameSpace validation");
            Assert.That(listOfLines[0], Is.EqualTo("namespace Expressium.Coffeeshop.Web.API.Pages"), "CodeGeneratorPageCSharp GenerateNameSpace validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateClass()
        {
            var listOfLines = codeGeneratorPage.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public partial class LoginPage : FramePage"), "CodeGeneratorPageCSharp GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMembers()
        {
            var listOfLines = codeGeneratorPage.GenerateMembers(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar Menu { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMembers_With_Table()
        {
            var tabelPage = page.Copy();
            tabelPage.AddControl(new ObjectRepositoryControl() { Name = "Grid", Type = "Table", How = "Id", Using = "products" });

            var listOfLines = codeGeneratorPage.GenerateMembers(tabelPage);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[0], Is.EqualTo("public MainMenuBar Menu { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
            Assert.That(listOfLines[1], Is.EqualTo("public BaseTable Grid { get; private set; }"), "CodeGeneratorPageCSharp GenerateMembers validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateLocators()
        {
            var listOfLines = codeGeneratorPage.GenerateLocators(page);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageCSharp GenerateLocators validation");
            Assert.That(listOfLines[0], Is.EqualTo("private WebTextBox Username => new WebTextBox(driver, By.Id(\"username\"));"), "CodeGeneratorPageCSharp GenerateLocators validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateContructor()
        {
            var listOfLines = codeGeneratorPage.GenerateContructor(page);

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

            var listOfLines = codeGeneratorPage.GenerateContructor(tabelPage);

            Assert.That(listOfLines.Count, Is.EqualTo(8), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[0], Is.EqualTo("public LoginPage(ILog logger, IWebDriver driver) : base(logger, driver)"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[2], Is.EqualTo("Menu = new MainMenuBar(logger, driver);"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[3], Is.EqualTo("Grid = new BaseTable(logger, driver, By.Id(\"products\"));"), "CodeGeneratorPageCSharp GenerateContructor validation");
            Assert.That(listOfLines[5], Is.EqualTo("WaitForPageTitleEquals(\"Home\");"), "CodeGeneratorPageCSharp GenerateContructor validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethods()
        {
            var listOfLines = codeGeneratorPage.GenerateActionMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetUsername(string value)"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("logger.Info($\"SetUsername({value})\");"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[3], Is.EqualTo("Username.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetUsername()"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[8], Is.EqualTo("logger.Info(\"GetUsername()\");"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Username.GetText();"), "CodeGeneratorPageCSharp GenerateActionMethods validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateFillFormMethod()
        {
            var listOfLines = codeGeneratorPage.GenerateFillFormMethod(page);

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
