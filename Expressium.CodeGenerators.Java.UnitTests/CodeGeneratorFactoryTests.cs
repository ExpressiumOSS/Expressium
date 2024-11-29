using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.UnitTests
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
        public void CodeGeneratorFactoryJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorFactory.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(22), "CodeGeneratorFactoryJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorFactoryJava_GenerateImports()
        {
            var listOfLines = codeGeneratorFactory.GenerateImports(page);

            Assert.That(listOfLines.Count, Is.EqualTo(4), "CodeGeneratorFactoryJava GenerateImports validation");
            Assert.That(listOfLines[0], Is.EqualTo("package Factories;"), "CodeGeneratorFactoryJava GenerateImports validation");
            Assert.That(listOfLines[2], Is.EqualTo("import Models.RegistrationPageModel;"), "CodeGeneratorFactoryJava GenerateImports validation");
        }

        [Test]
        public void CodeGeneratorFactoryJava_GenerateAttributes()
        {
            var listOfLines = codeGeneratorFactory.GenerateDefaultMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(15), "CodeGeneratorFactoryJava GenerateAttributes validation");
            Assert.That(listOfLines[0], Is.EqualTo("public static RegistrationPageModel Default()"), "CodeGeneratorFactoryJava GenerateDefaultMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("RegistrationPageModel model = new RegistrationPageModel();"), "CodeGeneratorFactoryJava GenerateDefaultMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("model.setFirstName(\"Hugoline\");"), "CodeGeneratorFactoryJava GenerateDefaultMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("model.setMale(false);"), "CodeGeneratorFactoryJava GenerateDefaultMethod validation");
        }
    }
}
