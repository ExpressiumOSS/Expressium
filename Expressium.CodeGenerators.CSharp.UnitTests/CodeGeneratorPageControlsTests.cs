using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.CSharp.UnitTests
{
    [TestFixture]
    public class CodeGeneratorPageControlsTests
    {
        CodeGeneratorPage codeGeneratorPage;

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            codeGeneratorPage = new CodeGeneratorPage(configuration, new ObjectRepository());
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateByLocatorsLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPage.GenerateByLocatorsLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateByLocatorsLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private readonly By Search = By.Id(\"search\");"), "CodeGeneratorPageCSharp GenerateByLocatorsLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateByLocatorsLocator_with_Double_Quotes()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "XPath";
            control.Using = "//a[text()=\"LM001 - Bank's consolidated LOM position\"]";

            var listOfLines = codeGeneratorPage.GenerateByLocatorsLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateByLocatorsLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private readonly By Search = By.XPath(\"//a[text()=\\\"LM001 - Bank's consolidated LOM position\\\"]\");"), "CodeGeneratorPageCSharp GenerateByLocatorsLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetSearch(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Search.SetTextBox(driver, value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetSearch()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Search.GetTextBox(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetYes(bool value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Yes.SetRadioButton(driver, value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetYes()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Yes.GetRadioButton(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetAgreed(bool value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Agreed.SetCheckBox(driver, value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetAgreed()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Agreed.GetCheckBox(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetComboBox(driver, value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetComboBox(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetListBox(driver, value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetListBox(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickAboutUs()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("AboutUs.ClickLink(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickSubmit()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Submit.ClickButton(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public string GetHeading()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return Heading.GetText(driver);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }
    }
}
