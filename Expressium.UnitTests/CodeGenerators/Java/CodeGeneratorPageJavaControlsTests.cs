using Expressium.CodeGenerators.Java;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.UnitTests.CodeGenerators.Java
{
    [TestFixture]
    public class CodeGeneratorPageJavaControlsTests
    {
        private CodeGeneratorPageJava codeGeneratorPageJava;

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            codeGeneratorPageJava = new CodeGeneratorPageJava(configuration, new ObjectRepository());
        }

        [TestCase("Id", "ID")]
        [TestCase("Name", "NAME")]
        [TestCase("ClassName", "CLASS_NAME")]
        [TestCase("CssSelector", "CSS")]
        [TestCase("XPath", "XPATH")]
        [TestCase("LinkText", "LINK_TEXT")]
        [TestCase("PartialLinkText", "PARTIAL_LINK_TEXT")]
        [TestCase("TagName", "TAG_NAME")]
        public void CodeGeneratorPageJava_GetFindbysHowMapping(string input, string expected)
        {
            var value = codeGeneratorPageJava.GetFindbysHowMapping(input);
            Assert.That(value, Is.EqualTo(expected), "CodeGeneratorPageJava GetFindbysHowMapping validate mapping...");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateFindsByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPageJava.GenerateFindsByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorPageJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("@FindBy(how = How.ID, using = \"search\")"), "CodeGeneratorPageJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[1], Is.EqualTo("private WebElement search;"), "CodeGeneratorPageJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[2], Is.EqualTo(""), "CodeGeneratorPageJava GenerateFindsByLocator validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPageJava.GenerateByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJava GenerateByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private By search = By.id(\"search\");"), "CodeGeneratorPageJava GenerateByLocator validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setSearch(String value) throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setTextBox(driver, search, value);"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getSearch() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getTextBox(driver, search);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setYes(boolean value) throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setRadioButton(driver, yes, value);"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getYes() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getRadioButton(driver, yes);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setAgreed(boolean value) throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setCheckBox(driver, agreed, value);"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getAgreed() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getCheckBox(driver, agreed);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setComboBox(driver, product, value);"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getComboBox(driver, product);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setListBox(driver, product, value);"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getListBox(driver, product);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickAboutUs() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickLink(driver, aboutUs);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickSubmit() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickButton(driver, submit);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public String getHeading() throws Exception"), "CodeGeneratorPageJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return WebElements.getText(driver, heading);"), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_Table_Without_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorPageJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateMethod_Table_With_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";
            control.Value = "Id;Description;Price";

            var listOfLines = codeGeneratorPageJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorPageJava GenerateMethod validation");
        }
    }
}
