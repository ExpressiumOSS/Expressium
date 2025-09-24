using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.CSharp.Playwright.UnitTests
{
    [TestFixture]
    public class CodeGeneratorFactoryTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorFactory codeGeneratorFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            page = new ObjectRepositoryPage();
            page.Name = "RegistrationPage";
            page.Title = "Registration";
            page.Model = true;
            page.Controls.Add(new ObjectRepositoryControl() { Name = "FirstName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "firstname", Value = "Hugoline" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "LastName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "lastname" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Country", Type = ControlTypes.ComboBox.ToString(), How = ControlHows.Name.ToString(), Using = "country", Value = "Denmark" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Male", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_0", Value = "False" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Female", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_1", Value = "True" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "IAgreeToTheTermsOfUse", Type = ControlTypes.CheckBox.ToString(), How = ControlHows.Name.ToString(), Using = "agreement" });

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorFactory = new CodeGeneratorFactory(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorFactoryCSharp_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorFactory.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(24), "CodeGeneratorFactoryCSharp GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorFactoryCSharp_GenerateUsings()
        {
            var listOfLines = codeGeneratorFactory.GenerateUsings();

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorFactoryCSharp GenerateUsings validation");
            Assert.That(listOfLines[0], Is.EqualTo("using Expressium.Coffeeshop.Web.API.Models;"), "CodeGeneratorFactoryCSharp GenerateUsings validation");
            Assert.That(listOfLines[1], Is.EqualTo("using System;"), "CodeGeneratorFactoryCSharp GenerateUsings validation");
        }

        [Test]
        public void codeGeneratorFactoryCSharp_GenerateNameSpace()
        {
            var listOfLines = codeGeneratorFactory.GenerateNameSpace();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "codeGeneratorFactoryCSharp GenerateNameSpace validation");
            Assert.That(listOfLines[0], Is.EqualTo("namespace Expressium.Coffeeshop.Web.API.Tests.Factories"), "codeGeneratorFactoryCSharp GenerateNameSpace validation");
        }

        [Test]
        public void codeGeneratorFactoryCSharp_GenerateClass()
        {
            var listOfLines = codeGeneratorFactory.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "codeGeneratorFactoryCSharp GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class RegistrationPageModelFactory"), "codeGeneratorFactoryCSharp GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorFactoryCSharp_GenerateAttributes()
        {
            var listOfLines = codeGeneratorFactory.GenerateDefaultMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(15), "CodeGeneratorFactoryCSharp GenerateAttributes validation");
            Assert.That(listOfLines[0], Is.EqualTo("public static RegistrationPageModel Default()"), "CodeGeneratorFactoryCSharp GenerateDefaultMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("var model = new RegistrationPageModel();"), "CodeGeneratorFactoryCSharp GenerateDefaultMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("model.FirstName = \"Hugoline\";"), "CodeGeneratorFactoryCSharp GenerateDefaultMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("model.Male = false;"), "CodeGeneratorFactoryCSharp GenerateDefaultMethod validation");
        }
    }
}
