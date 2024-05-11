using NUnit.Framework;
using Expressium.CodeGenerators.CSharp;
using Expressium.ObjectRepositories;
using Expressium.Configurations;

namespace Expressium.UnitTests.CodeGenerators.CSharp
{
    [TestFixture]
    public class CodeGeneratorPageCSharpControlsTests
    {
        CodeGeneratorPageCSharp codeGeneratorPageCSharp;

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            codeGeneratorPageCSharp = new CodeGeneratorPageCSharp(configuration, new ObjectRepository());
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateFindsByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPageCSharp.GenerateFindsByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("[FindsBy(How = How.Id, Using = \"search\")]"), "CodeGeneratorPageCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[1], Is.EqualTo("private IWebElement Search { get; set; }"), "CodeGeneratorPageCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[2], Is.EqualTo(""), "CodeGeneratorPageCSharp GenerateFindsByLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPageCSharp.GenerateByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageCSharp GenerateByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private readonly By Search = By.Id(\"search\");"), "CodeGeneratorPageCSharp GenerateByLocator validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetSearch(string value)"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Search.SetTextBox(driver, value);"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetSearch()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Search.GetTextBox(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetYes(bool value)"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Yes.SetRadioButton(driver, value);"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetYes()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Yes.GetRadioButton(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetAgreed(bool value)"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Agreed.SetCheckBox(driver, value);"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetAgreed()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Agreed.GetCheckBox(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetComboBox(driver, value);"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetComboBox(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetListBox(driver, value);"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetListBox(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickAboutUs()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("AboutUs.ClickLink(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickSubmit()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Submit.ClickButton(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageCSharp_GenerateMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = codeGeneratorPageCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public string GetHeading()"), "CodeGeneratorPageCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return Heading.GetText(driver);"), "CodeGeneratorPageCSharp GenerateMethod validation");
        }
    }
}
