using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Playwright.UnitTests
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
        public void CodeGeneratorFactoryJavaPlaywright_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorFactory.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(21), "CodeGeneratorFactoryJavaPlaywright GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorFactoryJavaPlaywright_GeneratePackage()
        {
            var listOfLines = codeGeneratorFactory.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorFactoryJavaPlaywright GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.factories;"), "CodeGeneratorFactoryJavaPlaywright GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorFactoryJavaPlaywright_GenerateClass()
        {
            var listOfLines = codeGeneratorFactory.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorFactoryJavaPlaywright GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class RegistrationPageModelFactory"), "CodeGeneratorFactoryJavaPlaywright GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorFactoryJavaPlaywright_GenerateDefaultMethod()
        {
            var listOfLines = codeGeneratorFactory.GenerateDefaultMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(14), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public static RegistrationPageModel getDefault() {"), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
            Assert.That(listOfLines[1], Is.EqualTo("RegistrationPageModel model = new RegistrationPageModel();"), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
            Assert.That(listOfLines[5], Is.EqualTo("model.setFirstName(\"Hugoline\");"), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
            Assert.That(listOfLines[8], Is.EqualTo("model.setMale(false);"), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("model.setFemale(true);"), "CodeGeneratorFactoryJavaPlaywright GenerateDefaultMethod validation");
        }
    }
}
