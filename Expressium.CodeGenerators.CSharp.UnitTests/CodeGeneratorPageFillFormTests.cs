using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Expressium.CodeGenerators.CSharp.UnitTests
{
    [TestFixture]
    public class CodeGeneratorPageFillFormTests
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

            page = CreateFillFormPage();

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorPage = new CodeGeneratorPage(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateFillFormMethod()
        {
            var listOfLines = codeGeneratorPage.GenerateFillFormMethod(page);

            Assert.That(listOfLines.Count, Is.EqualTo(9), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void FillForm(FillFormPageModel model)"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[2], Is.EqualTo("SetUsername(model.Username);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("SetGender(model.Gender);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[4], Is.EqualTo("SetTransport(model.Transport);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[5], Is.EqualTo("SetAgreement(model.Agreement);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("SetSection(model.Section);"), "CodeGeneratorPageCSharp GenerateFillFormMethod validation");
        }

        private static ObjectRepositoryPage CreateFillFormPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "FillFormPage";
            page.Model = true;

            var username = new ObjectRepositoryControl();
            username.Name = "Username";
            username.Type = "TextBox";
            username.How = "Id";
            username.Using = "username";
            page.AddControl(username);

            var gender = new ObjectRepositoryControl();
            gender.Name = "Gender";
            gender.Type = "ComboBox";
            gender.How = "Id";
            gender.Using = "gender";
            page.AddControl(gender);

            var transport = new ObjectRepositoryControl();
            transport.Name = "Transport";
            transport.Type = "ListBox";
            transport.How = "Id";
            transport.Using = "transport";
            page.AddControl(transport);

            var agreement = new ObjectRepositoryControl();
            agreement.Name = "Agreement";
            agreement.Type = "CheckBox";
            agreement.How = "Id";
            agreement.Using = "agreement";
            page.AddControl(agreement);

            var section = new ObjectRepositoryControl();
            section.Name = "Section";
            section.Type = "RadioButton";
            section.How = "Id";
            section.Using = "section";
            page.AddControl(section);

            return page;
        }
    }
}
