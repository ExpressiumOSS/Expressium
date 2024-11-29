using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.CodeGenerators.Java.UnitTests
{
    [TestFixture]
    public class CodeGeneratorPageControlsTests
    {
        private CodeGeneratorPage codeGeneratorPage;

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            codeGeneratorPage = new CodeGeneratorPage(configuration, new ObjectRepository());
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
            var value = codeGeneratorPage.GetFindbysHowMapping(input);
            Assert.That(value, Is.EqualTo(expected), "CodeGeneratorPageJava GetFindbysHowMapping validate mapping...");
        }

        [Test]
        public void CodeGeneratorPageJava_GeneratePageFactoryLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPage.GeneratePageFactoryLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(2), "CodeGeneratorPageJava GeneratePageFactoryLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("@FindBy(how = How.ID, using = \"search\")"), "CodeGeneratorPageJava GeneratePageFactoryLocator validation");
            Assert.That(listOfLines[1], Is.EqualTo("private WebElement search;"), "CodeGeneratorPageJava GeneratePageFactoryLocator validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateByLocatorsLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = codeGeneratorPage.GenerateByLocatorsLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorPageJava GenerateByLocatorsLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private By search = By.id(\"search\");"), "CodeGeneratorPageJava GenerateByLocatorsLocator validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setSearch(String value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setTextBox(driver, search, value);"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getSearch() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getTextBox(driver, search);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setYes(boolean value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setRadioButton(driver, yes, value);"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getYes() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getRadioButton(driver, yes);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setAgreed(boolean value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setCheckBox(driver, agreed, value);"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getAgreed() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getCheckBox(driver, agreed);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setComboBox(driver, product, value);"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getComboBox(driver, product);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setListBox(driver, product, value);"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getListBox(driver, product);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickAboutUs() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickLink(driver, aboutUs);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickSubmit() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickButton(driver, submit);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public String getHeading() throws Exception"), "CodeGeneratorPageJava GenerateActionMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return WebElements.getText(driver, heading);"), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_Table_Without_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorPageJava GenerateActionMethod validation");
        }

        [Test]
        public void CodeGeneratorPageJava_GenerateActionMethod_Table_With_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";
            control.Value = "Id;Description;Price";

            var listOfLines = codeGeneratorPage.GenerateActionMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorPageJava GenerateActionMethod validation");
        }
    }
}
