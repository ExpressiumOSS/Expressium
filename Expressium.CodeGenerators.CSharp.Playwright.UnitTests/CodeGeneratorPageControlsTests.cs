using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.CSharp.Playwright.UnitTests
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
            Assert.That(listOfLines[0], Is.EqualTo("private Web Search => new Web(page, page.Locator(\"[id='search']\"));"), "CodeGeneratorPageCSharp GenerateLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateLocator_with_Double_Quotes()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "XPath";
            control.Type = "Link";
            control.Using = "//a[text()=\"LM001 - Bank's consolidated LOM position\"]";

            var listOfLines = codeGeneratorPage.GenerateLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private WebLink Search => new WebLink(page, page.Locator(\"//a[text()=\\\"LM001 - Bank's consolidated LOM position\\\"]\"));"), "CodeGeneratorPageCSharp GenerateLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task SetSearch(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Search.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public async Task<string> GetSearch()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return await Search.GetText();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task SetYes(bool value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Yes.SetSelected(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public async Task<bool> GetYes()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return await Yes.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task SetAgreed(bool value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Agreed.SetChecked(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public async Task<bool> GetAgreed()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return await Agreed.GetChecked();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Product.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public async Task<string> GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return await Product.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Product.SetText(value);"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public async Task<string> GetProduct()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return await Product.GetSelected();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task ClickAboutUs()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await AboutUs.Click();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task ClickSubmit()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("await Submit.Click();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateActionMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public async Task<string> GetHeading()"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return await Heading.GetText();"), "CodeGeneratorPageCSharp GenerateActionMethod validation");
        }
    }
}
