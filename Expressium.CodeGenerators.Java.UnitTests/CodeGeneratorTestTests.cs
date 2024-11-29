using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.UnitTests
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
        public void CodeGeneratorPageJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorTest.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(38), "CodeGeneratorPageJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateImports()
        {
            var listOfLines = codeGeneratorTest.GenerateImports();

            Assert.That(listOfLines.Count, Is.EqualTo(10), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[0], Is.EqualTo("package UITests;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import Bases.Asserts;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[3], Is.EqualTo("import Bases.BaseTest;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[4], Is.EqualTo("import Factories.*;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[5], Is.EqualTo("import Models.*;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[6], Is.EqualTo("import Pages.*;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[7], Is.EqualTo("import org.testng.annotations.BeforeClass;"), "CodeGeneratorPageJava GenerateImports validation");
            Assert.That(listOfLines[8], Is.EqualTo("import org.testng.annotations.Test;"), "CodeGeneratorPageJava GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GeneratedDataVariables()
        {
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJava GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorPageJava GeneratedDataVariables validation");
            Assert.That(listOfLines[1], Is.EqualTo("private LoginPageModel loginPageModel = LoginPageModelFactory.Default();"), "CodeGeneratorPageJava GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GeneratedDataVariables_As_Varibles()
        {
            page.Model = false;
            var listOfLines = codeGeneratorTest.GeneratedDataVariables(page);
            page.Model = true;

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorPageJava GeneratedDataVariables validation");
            Assert.That(listOfLines[0], Is.EqualTo("private LoginPage loginPage;"), "CodeGeneratorPageJava GeneratedDataVariables validation");
            Assert.That(listOfLines[2], Is.EqualTo("private String username = \"Andersen\";"), "CodeGeneratorPageJava GeneratedDataVariables validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateOneTimeSetUpMethod()
        {
            var listOfLines = codeGeneratorTest.GenerateOneTimeSetUpMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(11), "CodeGeneratorPageJava GenerateOneTimeSetUpMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("initializeBrowser();"), "CodeGeneratorPageJava GenerateOneTimeSetUpMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GeneratePageTitleTestMethod()
        {
            var listOfLines = codeGeneratorTest.GeneratePageTitleTestMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorPageJava GeneratePageTitleTestMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Asserts.assertEquals(\"Login\", loginPage.getTitle(), \"Validating the LoginPage Title...\");"), "CodeGeneratorPageJava GeneratePageTitleTestMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateFillFormTestMethods()
        {
            var listOfLines = codeGeneratorTest.GenerateFillFormTestMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("public void validate_Page_Property_Username() throws Exception"), "CodeGeneratorPageJava GenerateFillFormTestMethods validation");
            Assert.That(listOfLines[4].Contains("assertEquals(loginPageModel.getUsername(), loginPage.getUsername()"), "CodeGeneratorPageJava GenerateFillFormTestMethods validation");
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
