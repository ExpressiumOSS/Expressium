using NUnit.Framework;
using Expressium.CodeGenerators.Java;
using Expressium.Configurations;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.CodeGenerators.Java
{
    [TestFixture]
    public class CodeGeneratorModelJavaTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorModelJava codeGeneratorModelJava;

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

            codeGeneratorModelJava = new CodeGeneratorModelJava(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorModel_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorModelJava.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(75), "CodeGeneratorModel GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorModel_GenerateAttributes()
        {
            var listOfLines = codeGeneratorModelJava.GenerateAttributes(page);

            Assert.That(listOfLines.Count, Is.EqualTo(7), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[0], Is.EqualTo("private String firstName;"), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[1], Is.EqualTo("private String lastName;"), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[2], Is.EqualTo("private String country;"), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[3], Is.EqualTo("private boolean male;"), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[4], Is.EqualTo("private boolean female;"), "CodeGeneratorModel GenerateAttributes validation");
            Assert.That(listOfLines[5], Is.EqualTo("private boolean iAgreeToTheTermsOfUse;"), "CodeGeneratorModel GenerateAttributes validation");
        }

        [Test]
        public void CodeGeneratorModel_GenerateMethods_TextBox()
        {
            var listOfLines = codeGeneratorModelJava.GenerateMethods(page);

            Assert.That(listOfLines.Count, Is.EqualTo(60), "CodeGeneratorModel GenerateMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("public String getFirstName()"), "CodeGeneratorModel GenerateMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("return firstName;"), "CodeGeneratorModel GenerateMethods validation");
        }      
    }
}
