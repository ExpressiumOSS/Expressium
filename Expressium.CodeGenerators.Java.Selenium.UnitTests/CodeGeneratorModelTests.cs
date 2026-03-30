using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.Selenium.UnitTests
{
    [TestFixture]
    public class CodeGeneratorModelTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorModel codeGeneratorModel;

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
            page.Controls.Add(new ObjectRepositoryControl() { Name = "FirstName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "firstname" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "LastName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "lastname" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Country", Type = ControlTypes.ComboBox.ToString(), How = ControlHows.Name.ToString(), Using = "country", Value = "Denmark" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Male", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_0", Value = "False" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Female", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_1", Value = "True" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "IAgreeToTheTermsOfUse", Type = ControlTypes.CheckBox.ToString(), How = ControlHows.Name.ToString(), Using = "agreement" });

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorModel = new CodeGeneratorModel(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorModelJava_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorModel.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(63), "CodeGeneratorModelJava GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorModelJava_GeneratePackage()
        {
            var listOfLines = codeGeneratorModel.GeneratePackage();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorModelJava GeneratePackage validation");
            Assert.That(listOfLines[0], Is.EqualTo("package expressium.coffeeshop.web.api.models;"), "CodeGeneratorModelJava GeneratePackage validation");
        }

        [Test]
        public void CodeGeneratorModelJava_GenerateClass()
        {
            var listOfLines = codeGeneratorModel.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorModelJava GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class RegistrationPageModel"), "CodeGeneratorModelJava GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorModelJava_GenerateFields()
        {
            var listOfLines = codeGeneratorModel.GenerateFields(page);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[0], Is.EqualTo("private String firstName;"), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[1], Is.EqualTo("private String lastName;"), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[2], Is.EqualTo("private String country;"), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[3], Is.EqualTo("private boolean male;"), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[4], Is.EqualTo("private boolean female;"), "CodeGeneratorModelJava GenerateFields validation");
            Assert.That(listOfLines[5], Is.EqualTo("private boolean iAgreeToTheTermsOfUse;"), "CodeGeneratorModelJava GenerateFields validation");
        }

        [Test]
        public void CodeGeneratorModelJava_GenerateAccessors()
        {
            var listOfLines = codeGeneratorModel.GenerateAccessors(page);

            Assert.That(listOfLines.Count, Is.EqualTo(48), "CodeGeneratorModelJava GenerateAccessors validation");
            Assert.That(listOfLines[0], Is.EqualTo("public String getFirstName() {"), "CodeGeneratorModelJava GenerateAccessors validation");
            Assert.That(listOfLines[4], Is.EqualTo("public void setFirstName(String value) {"), "CodeGeneratorModelJava GenerateAccessors validation");
        }
    }
}
