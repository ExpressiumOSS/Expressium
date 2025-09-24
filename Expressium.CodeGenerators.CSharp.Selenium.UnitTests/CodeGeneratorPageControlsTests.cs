using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.CSharp.Selenium.UnitTests
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
        public void CodeGeneratorPageCSharp_GenerateLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPage.GenerateLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private Web Search => new Web(driver, By.Id(\"search\"));"), "CodeGeneratorPageCSharp GenerateLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateLocator_with_Double_Quotes()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "XPath";
            control.Using = "//a[text()=\"LM001 - Bank's consolidated LOM position\"]";

            var listOfLines = codeGeneratorPage.GenerateLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private Web Search => new Web(driver, By.XPath(\"//a[text()=\\\"LM001 - Bank's consolidated LOM position\\\"]\"));"), "CodeGeneratorPageCSharp GenerateLocator validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Search.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetSearch()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Search.GetText();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Yes.SetSelected(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetYes()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Yes.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Agreed.SetChecked(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetAgreed()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Agreed.GetChecked();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("AboutUs.Click();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("Submit.Click();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
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
            Assert.That(listOfLines[3], Is.EqualTo("return Heading.GetText();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }
    }
}
